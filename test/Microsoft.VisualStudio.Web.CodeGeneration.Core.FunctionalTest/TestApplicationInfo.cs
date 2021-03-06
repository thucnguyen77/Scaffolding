// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.Web.CodeGeneration.DotNet;

namespace Microsoft.VisualStudio.Web.CodeGeneration.Core.FunctionalTest
{
    // Represents an application applicationInfo that overrides the base path of the original
    // application applicationInfo in order to make it point to the test application folder.
    public class TestApplicationInfo : IApplicationInfo
    {
        private readonly IApplicationInfo _originalAppEnvironment;
        private readonly string _applicationBasePath;
        private readonly string _appName;
        private readonly string _appConfiguration;

        public TestApplicationInfo(IApplicationInfo originalAppEnvironment,
            string appBasePath, string appName)
        {
            _originalAppEnvironment = originalAppEnvironment;
            _applicationBasePath = appBasePath;
            _appName = appName;
            _appConfiguration = originalAppEnvironment.ApplicationConfiguration;
        }

        public string ApplicationName
        {
            get { return _appName; }
        }

        public string ApplicationBasePath
        {
            get { return _applicationBasePath; }
        }

        public string ApplicationConfiguration
        {
            get { return _appConfiguration; }
        }

        public object GetData(string name)
        {
            throw new NotImplementedException();
        }

        public void SetData(string name, object value)
        {
            throw new NotImplementedException();
        }
    }
}