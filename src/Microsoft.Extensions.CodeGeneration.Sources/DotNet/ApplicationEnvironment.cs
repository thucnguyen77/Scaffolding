using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.CodeGeneration.Sources.DotNet
{
    public class ApplicationEnvironment : IApplicationEnvironment
    {
        public string ApplicationBasePath
        {
            get
            {
                return AppContext.BaseDirectory;
            }
        }

        public string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
