// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.Extensions.CodeGeneration.Sources.DotNet;
using System.Runtime.Loader;

namespace Microsoft.Extensions.CodeGeneration
{
    internal static class CommonUtilities
    {
        public static CompilationResult GetAssemblyFromCompilation(
            AssemblyLoadContext loader,
            CodeAnalysis.Compilation compilation)
        {
            var assemblyName = Path.GetRandomFileName()+".dll";
            EmitResult result;
            using (var ms = new MemoryStream())
            {
                using (var pdb = new MemoryStream())
                {
                    if (PlatformHelper.IsMono)
                    {
                        result = compilation.Emit(ms, pdbStream: null);
                    }
                    else
                    {
                        result = compilation.Emit(ms, pdbStream: pdb);
                    }

                    if (!result.Success)
                    {
                        var formatter = new DiagnosticFormatter();
                        var errorMessages = result.Diagnostics
                                             .Where(IsError)
                                             .Select(d => formatter.Format(d));

                        return CompilationResult.FromErrorMessages(errorMessages);
                    }

                    ms.Seek(0, SeekOrigin.Begin);
                    
                    Assembly assembly;
                    //TODO: @prbhosal Fix this 
                    using (var writer = new BinaryWriter(File.Open(assemblyName, FileMode.CreateNew)))
                    {
                        int length = 4096;
                        int read = 0;
                        do
                        {
                            byte[] buff = new byte[length];
                            read = ms.Read(buff, 0, length);
                            if (read > 0)
                            {
                                writer.Write(buff);
                            }
                        } while (read > 0);
                    }

                    if (PlatformHelper.IsMono)
                    {
                        //TODO: @prbhosal Fix this 
                        assembly = loader.LoadFromAssemblyName(new AssemblyName(compilation.AssemblyName));
                        //assembly = loader.LoadStream(ms, assemblySymbols: null);
                    }
                    else
                    {
                        //TODO: @prbhosal Fix this 
                        pdb.Seek(0, SeekOrigin.Begin);
                        assembly = loader.LoadFromAssemblyName(new AssemblyName(compilation.AssemblyName));
                    }

                    return CompilationResult.FromAssembly(assembly);
                }
            }
        }

        private static bool IsError(Diagnostic diagnostic)
        {
            return diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error;
        }
    }
}