using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.DotNet.ProjectModel.Compilation;
using Microsoft.DotNet.ProjectModel;

namespace Microsoft.Extensions.CodeGeneration.Sources.DotNet
{
    public class LibraryExporter : ILibraryExporter
    {
        private Microsoft.DotNet.ProjectModel.Compilation.LibraryExporter _libraryExporter;

        public LibraryExporter(ProjectContext context)
        {
            if(context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            //TODO @prbhosal validate this
            _libraryExporter = context.CreateExporter("Debug");
        }
        public IEnumerable<LibraryExport> GetAllExports()
        {
            throw new NotImplementedException();
        }

        public LibraryExport GetExport(string name)
        {
            throw new NotImplementedException();
        }
    }
}
