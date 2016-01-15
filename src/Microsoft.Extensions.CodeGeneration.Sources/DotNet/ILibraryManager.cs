using Microsoft.DotNet.ProjectModel;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.CodeGeneration.Sources.DotNet
{
    public interface ILibraryManager
    {
        IEnumerable<LibraryDescription> GetLibraries();
        LibraryDescription GetLibrary(string name);
        IEnumerable<LibraryDescription> GetReferencingLibraries(string name);
    }
}
