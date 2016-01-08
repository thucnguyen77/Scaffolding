using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.DotNet.ProjectModel;

namespace Microsoft.Extensions.CodeGeneration.Sources.DotNet
{
    public class LibraryManager : ILibraryManager
    {
        private Microsoft.DotNet.ProjectModel.Resolution.LibraryManager _libraryManager;

        public LibraryManager(ProjectContext projectContext)
        {
            if(projectContext == null)
            {
                throw new ArgumentNullException(nameof(projectContext));
            }
            _libraryManager = projectContext.LibraryManager;
        }


        public IEnumerable<LibraryDescription> GetLibraries()
        {
            return _libraryManager.GetLibraries();
        }

        public LibraryDescription GetLibrary(string name)
        {
            return _libraryManager.GetLibraries().Where(_ => _.Identity.Name == name).FirstOrDefault();
        }

        public IEnumerable<LibraryDescription> GetReferencingLibraries(string name)
        {
            // Get all libraries where the dependencies of the library contains 'name' as a dependency.
            //_libraryManager.GetLibraries().Where(_ => _.Dependencies)
            throw new NotImplementedException();
        }
    }
}
