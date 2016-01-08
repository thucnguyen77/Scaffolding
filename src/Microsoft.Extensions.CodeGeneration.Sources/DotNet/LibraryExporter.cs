using System;
using System.Collections.Generic;
using Microsoft.DotNet.ProjectModel.Compilation;
using Microsoft.DotNet.ProjectModel;
using System.Linq;

namespace Microsoft.Extensions.CodeGeneration.Sources.DotNet
{
    public class LibraryExporter : ILibraryExporter
    {
        //private DotNet.ProjectModel.Resolution
        private Microsoft.DotNet.ProjectModel.Compilation.LibraryExporter _libraryExporter;

        public LibraryExporter(ProjectContext projectContext)
        {
            if(projectContext == null)
            {
                throw new ArgumentNullException(nameof(projectContext));
            }
            _libraryExporter = projectContext.CreateExporter("");
        }

        public IEnumerable<LibraryExport> GetAllExports()
        {
            return _libraryExporter.GetAllExports();
        }

        public LibraryExport GetExport(string name)
        {
            return _libraryExporter.GetAllExports().Where(_ => _.Library.Identity.Name == name).FirstOrDefault();
        }
    }
}
