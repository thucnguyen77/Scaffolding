using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.CodeGeneration.EntityFramework;
using Microsoft.Extensions.CodeGeneration.Templating;
using Microsoft.Extensions.CodeGeneration.Templating.Compilation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.CodeGeneration.Sources.DotNet;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.DotNet.ProjectModel;
using System.Runtime.Loader;
using System.IO;

namespace Microsoft.Extensions.CodeGeneration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            PrintCommandLine(args);
            var app = new CommandLineApplication(false)
            {
                Name = "Code Generation",
                Description = "Code generation for Asp.net"
            };
            app.HelpOption("-h|--help");
            var projectPath = app.Option("-p|--project", "Path to project.json", CommandOptionType.SingleValue);

            

            app.OnExecute(() =>
            {
                PrintCommandLine(projectPath, app.RemainingArguments);
                var serviceProvider = new ServiceProvider();
                var context = CreateProjectContext(projectPath.Value());
                AddFrameworkServices(serviceProvider, context);
                AddCodeGenerationServices(serviceProvider);
                var codeGenCommand = new CodeGenCommand(serviceProvider);
                codeGenCommand.Execute(app.RemainingArguments.ToArray());
                return 1;
            });

            app.Execute(args);
        }

        private static void PrintCommandLine(CommandOption projectPath, List<string> remainingArguments)
        {
            Console.WriteLine("Command line After parsing :: ");
            if(projectPath != null)
            {
                Console.WriteLine(string.Format("    Project path: {0}", projectPath.Value()));
            }
            Console.WriteLine("    Remaining Args :: ");
            if(remainingArguments != null)
            {
                foreach (var arg in remainingArguments)
                {
                    Console.WriteLine("        "+arg);
                }
            }
        }

        private static void AddFrameworkServices(ServiceProvider serviceProvider, ProjectContext context)
        {
            serviceProvider.Add(typeof(IApplicationEnvironment),new ApplicationEnvironment());
            //serviceProvider.Add(typeof(AssemblyLoadContext), context.CreateLoadContext());
            serviceProvider.Add(typeof(ILibraryManager), new LibraryManager(context));
            serviceProvider.Add(typeof(ILibraryExporter), new LibraryExporter(context));
        }

        private static ProjectContext CreateProjectContext(string projectPath)
        {
            projectPath = projectPath ?? Directory.GetCurrentDirectory();

            if (!projectPath.EndsWith(Project.FileName))
            {
                projectPath = Path.Combine(projectPath, Project.FileName);
            }

            if (!File.Exists(projectPath))
            {
                throw new InvalidOperationException($"{projectPath} does not exist.");
            }

            return ProjectContext.CreateContextForEachFramework(projectPath).FirstOrDefault();
        }

        private static void PrintCommandLine(string []args)
        {
            Console.WriteLine("Raw command line ::");
            if(args != null)
            {
                foreach(string arg in args)
                {
                    Console.WriteLine("    "+arg);
                }
            }
            else
            {
                Console.WriteLine("No arguments!!! >-<");
            }
        }

        private static void AddCodeGenerationServices(ServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            //Ordering of services is important here
            serviceProvider.Add(typeof(ILogger), new ConsoleLogger());
            serviceProvider.Add(typeof(IFilesLocator), new FilesLocator());

            serviceProvider.AddServiceWithDependencies<ICodeGeneratorAssemblyProvider, DefaultCodeGeneratorAssemblyProvider>();
            serviceProvider.AddServiceWithDependencies<ICodeGeneratorLocator, CodeGeneratorsLocator>();

            serviceProvider.AddServiceWithDependencies<ICompilationService, RoslynCompilationService>();
            serviceProvider.AddServiceWithDependencies<ITemplating, RazorTemplating>();

            serviceProvider.AddServiceWithDependencies<IPackageInstaller, PackageInstaller>();

            serviceProvider.AddServiceWithDependencies<IModelTypesLocator, ModelTypesLocator>();
            serviceProvider.AddServiceWithDependencies<ICodeGeneratorActionsService, CodeGeneratorActionsService>();
            //serviceProvider.AddServiceWithDependencies<IDbContextEditorServices, DbContextEditorServices>();
            //serviceProvider.AddServiceWithDependencies<IEntityFrameworkService, EntityFrameworkServices>();
        }
    }
}
