using System;
using System.Collections.Generic;
using Microsoft.DotNet.ProjectModel;

namespace Microsoft.Extensions.CodeGeneration.Sources.DotNet
{
    public interface ILibraryManager
    {
        IEnumerable<LibraryDescription> GetLibraries();
        LibraryDescription GetLibrary(string name);
        IEnumerable<LibraryDescription> GetReferencingLibraries(string name);
    }
}
