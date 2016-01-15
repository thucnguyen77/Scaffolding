using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.CodeGeneration.Sources.DotNet
{
    public interface IApplicationEnvironment
    {
        string ApplicationBasePath { get; }
        string ApplicationName { get; }
    }
}
