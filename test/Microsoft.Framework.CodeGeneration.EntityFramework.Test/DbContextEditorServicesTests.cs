using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Data.Entity;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.CodeGeneration.Templating;
using Moq;
using Xunit;
using System.Diagnostics;
using System.IO;
using Microsoft.CodeAnalysis.Emit;

namespace Microsoft.Framework.CodeGeneration.EntityFramework.Test
{
    public class DbContextEditorServicesTests
    {
        [Theory]
        [InlineData("DbContext_Before.txt", "MyModel.txt", "DbContext_After.txt")]
        // Below test is failing because of a product bug, need to fix it and then enable.
        //[InlineData("EmptyDbContext_Before.txt", "MyModel.txt", "EmptyDbContext_After.txt")]
        public void AddModelToContext_Adds_Model_From_Same_Project_To_Context(string beforeContextResource, string modelResource, string afterContextResource)
        {
            // Arrange
            string resourcePrefix = "compiler/resources/";

            var beforeDbContextText = ResourceUtilities.GetEmbeddedResourceFileContent(resourcePrefix + beforeContextResource);
            var modelText = ResourceUtilities.GetEmbeddedResourceFileContent(resourcePrefix + modelResource);
            var afterDbContextText = ResourceUtilities.GetEmbeddedResourceFileContent(resourcePrefix + afterContextResource);

            var contextTree = CSharpSyntaxTree.ParseText(beforeDbContextText);
            var modelTree = CSharpSyntaxTree.ParseText(modelText);
            var efReference = MetadataReference.CreateFromFile(typeof(DbContext).Assembly.Location);

            var compilation = CSharpCompilation.Create("DoesNotMatter", new[] { contextTree, modelTree }, new[] { efReference });

            DbContextEditorServices testObj = new DbContextEditorServices(
                new Mock<ILibraryManager>().Object,
                new Mock<IApplicationEnvironment>().Object,
                new Mock<IFilesLocator>().Object,
                new Mock<ITemplating>().Object);

            var types = RoslynUtilities.GetDirectTypesInCompilation(compilation);
            var modelType = ModelType.FromITypeSymbol(types.Where(ts => ts.Name == "MyModel").First());
            var contextType = ModelType.FromITypeSymbol(types.Where(ts => ts.Name == "MyContext").First());

            // Act
            var result = testObj.AddModelToContext(contextType, modelType);

            // Assert
            Assert.True(result.Added);
            Assert.Equal(afterDbContextText, result.NewTree.GetText().ToString());
        }

        [Theory]
        [InlineData("DbContext_Before.txt", "MyModel.txt", "DbContext_After.txt")]
        public void AddModelToContext_Adds_Model_From_ProjectReference_To_Context(string beforeContextResource, string modelResource, string afterContextResource)
        {
            // Arrange
            string resourcePrefix = "compiler/resources/";

            var beforeDbContextText = ResourceUtilities.GetEmbeddedResourceFileContent(resourcePrefix + beforeContextResource);
            var modelText = ResourceUtilities.GetEmbeddedResourceFileContent(resourcePrefix + modelResource);
            var afterDbContextText = ResourceUtilities.GetEmbeddedResourceFileContent(resourcePrefix + afterContextResource);

            var contextTree = CSharpSyntaxTree.ParseText(beforeDbContextText);
            var modelTree = CSharpSyntaxTree.ParseText(modelText);
            var efReference = MetadataReference.CreateFromFile(typeof(DbContext).Assembly.Location);

            var modelProj = CSharpCompilation.Create("ModelProj", new[] { modelTree });
            var modelProjReference = modelProj.ToMetadataReference() as MetadataReference;
            var contextProj = CSharpCompilation.Create("DbContextProj", new[] { contextTree }, new[] { efReference, modelProjReference });
            
            DbContextEditorServices testObj = new DbContextEditorServices(
                new Mock<ILibraryManager>().Object,
                new Mock<IApplicationEnvironment>().Object,
                new Mock<IFilesLocator>().Object,
                new Mock<ITemplating>().Object);

            var modelType = ModelType.FromITypeSymbol(RoslynUtilities.GetDirectTypesInCompilation(modelProj)
                .Where(ts => ts.Name == "MyModel").First());
            var contextType = ModelType.FromITypeSymbol(RoslynUtilities.GetDirectTypesInCompilation(contextProj)
                .Where(ts => ts.Name == "MyContext").First());

            // Act
            var result = testObj.AddModelToContext(contextType, modelType);

            // Assert
            Assert.True(result.Added);
            Assert.Equal(afterDbContextText, result.NewTree.GetText().ToString());
        }
    }
}
