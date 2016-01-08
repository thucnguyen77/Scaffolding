using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.DotNet.ProjectModel.Compilation;
using System.Reflection.PortableExecutable;
using System.IO;

namespace Microsoft.Extensions.CodeGeneration.Sources.DotNet
{
    public static class LibraryExportExtensions
    {
        public static IEnumerable<MetadataReference> GetMetadataReferences(this LibraryExport export)
        {
            var references = new List<MetadataReference>();
            AssemblyMetadata assemblyMetadata;
            foreach (var lib in export.CompilationAssemblies)
            {
                using (var stream = File.OpenRead(lib.ResolvedPath))
                {
                    var moduleMetadata = ModuleMetadata.CreateFromStream(stream, PEStreamOptions.PrefetchMetadata);
                    assemblyMetadata = AssemblyMetadata.Create(moduleMetadata);
                    references.Add(assemblyMetadata.GetReference());
                }
            }
            return references;
        }
    }
}
