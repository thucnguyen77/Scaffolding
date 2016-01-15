// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.CodeGeneration.Sources.DotNet;
using Microsoft.DotNet.ProjectModel;
using System.Runtime.Loader;

namespace Microsoft.Extensions.CodeGeneration
{
    public class DefaultCodeGeneratorAssemblyProvider : ICodeGeneratorAssemblyProvider
    {
        private static readonly HashSet<string> _codeGenerationFrameworkAssemblies =
            new HashSet<string>(StringComparer.Ordinal)
            {
                "Microsoft.Extensions.CodeGeneration",
            };

        private readonly ILibraryManager _libraryManager;

        public DefaultCodeGeneratorAssemblyProvider(ILibraryManager libraryManager)
        {
            if (libraryManager == null)
            {
                throw new ArgumentNullException(nameof(libraryManager));
            }

            _libraryManager = libraryManager;
        }

        public IEnumerable<Assembly> CandidateAssemblies
        {
            get
            {
                //TODO @prbhosal This needs to look into the bin folder for the assemblies. 
                var list = _codeGenerationFrameworkAssemblies
                    .SelectMany(_libraryManager.GetReferencingLibraries)
                    .Distinct()
                    .Where(IsCandidateLibrary);
                foreach(var lib in list)
                {
                    Console.WriteLine(lib.Identity.Name + " " + lib.Path);
                }
                return list.Select(lib => Assembly.Load(new AssemblyName(lib.Identity.Name)));
            }
        }

        private bool IsCandidateLibrary(LibraryDescription library)
        {
            return !_codeGenerationFrameworkAssemblies.Contains(library.Identity.Name) && ("Project" != library.Identity.Type.Value);
        }
    }
}