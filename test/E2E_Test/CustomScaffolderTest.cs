using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace E2E_Test
{
    [Collection("ScaffoldingE2ECollection")]
    public class CustomScaffolderTest : E2ETestBase
    {
        public CustomScaffolderTest(ScaffoldingE2ETestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void TestCustomScaffolder()
        {
            var args = new string[]
            {
                "-p",
                testProjectPath,
                "-c",
                "debug",
                "custom",
                "NewCustomFile",
                "--relativeFolderPath",
                "CustomFolder"
            };
            Scaffold(args);
            var generatedPath = Path.Combine(testProjectPath, "CustomFolder", "NewCustomFile.txt");
            VerifyFileAndContent(generatedPath, Path.Combine("CustomCodeGenerator", "CustomFile.txt"));

            _fixture.FilesToCleanUp.Add(generatedPath);
        }
    }
}
