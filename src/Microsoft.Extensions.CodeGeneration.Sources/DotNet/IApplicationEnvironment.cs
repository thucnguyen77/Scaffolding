using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.CodeGeneration.Sources.DotNet
{
    public interface IApplicationEnvironment
    {
        string ApplicationBasePath { get; }
        string ApplicationName { get; }
    }
}
