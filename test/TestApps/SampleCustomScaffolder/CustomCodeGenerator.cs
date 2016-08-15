using Microsoft.VisualStudio.Web.CodeGeneration;
using Microsoft.VisualStudio.Web.CodeGeneration.CommandLine;
using Microsoft.VisualStudio.Web.CodeGeneration.Core;
using Microsoft.VisualStudio.Web.CodeGeneration.DotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SampleCustomScaffolder
{
    [CodeGenerator("custom", "Generate")]
    [Alias("custom")]
    public class CustomCodeGenerator
    {
        IApplicationInfo _applicationInfo;
        ICodeGeneratorActionsService _codeGeneratorActionsService;
        ILibraryManager _libraryManager;
        ILogger _logger;

        public CustomCodeGenerator(
            IApplicationInfo applicationInfo,
            ICodeGeneratorActionsService codeGeneratorActionsService,
            ILibraryManager libraryManger,
            ILogger logger)
        {
            if(applicationInfo == null)
            {
                throw new ArgumentNullException(nameof(applicationInfo));
            }

            if(codeGeneratorActionsService == null)
            {
                throw new ArgumentNullException(nameof(codeGeneratorActionsService));
            }

            if(libraryManger == null)
            {
                throw new ArgumentNullException(nameof(libraryManger));
            }

            if(logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _applicationInfo = applicationInfo;
            _codeGeneratorActionsService = codeGeneratorActionsService;
            _libraryManager = libraryManger;
            _logger = logger;
        }

        private IEnumerable<string> TemplateFolders
        {
            get
            {
                return TemplateFoldersUtilities.GetTemplateFolders(
                    containingProject: typeof(CustomCodeGenerator).GetTypeInfo().Assembly.GetName().Name,
                    applicationBasePath: _applicationInfo.ApplicationBasePath,
                    baseFolders: new[] { "Custom" },
                    libraryManager: _libraryManager);
            }
        }

        public async Task Generate(CustomCodeGeneratorModel model)
        {
            if(string.IsNullOrEmpty(model.CustomFileName))
            {
                throw new ArgumentException("Please specify the file name.");
            }
            if(!model.CustomFileName.EndsWith("txt", StringComparison.OrdinalIgnoreCase))
            {
                model.CustomFileName = model.CustomFileName + ".txt";
            }
            var outputPath = Path.Combine(_applicationInfo.ApplicationBasePath, model.RelativeFolderPath ?? "", model.CustomFileName);
            await _codeGeneratorActionsService.AddFileAsync(outputPath, Path.Combine(TemplateFolders.FirstOrDefault(), "CustomFile.txt"));
            _logger.LogMessage($"Added file: {Path.Combine(model.RelativeFolderPath, model.CustomFileName)}");
        }
    }
}
