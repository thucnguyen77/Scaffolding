using Microsoft.DotNet.ProjectModel.Compilation;
using System.Collections.Generic;

namespace Microsoft.Extensions.CodeGeneration.Sources.DotNet
{
    public interface ILibraryExporter
    {
        IEnumerable<LibraryExport> GetAllExports();
        LibraryExport GetExport(string name);
    }
}
