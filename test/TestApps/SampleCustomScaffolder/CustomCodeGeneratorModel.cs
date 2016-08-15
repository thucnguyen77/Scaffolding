using Microsoft.VisualStudio.Web.CodeGeneration.CommandLine;

namespace SampleCustomScaffolder
{
    public class CustomCodeGeneratorModel
    {
        public string CustomFileName { get; set; }
        [Option(Name = "relativeFolderPath", ShortName = "outDir", Description = "Specify the relative output folder path from project where the file needs to be generated, if not specified, file will be generated in the project folder")]
        public string RelativeFolderPath { get; set; }
    }
}