using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.Web.CodeGeneration.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CodeGeneratorAttribute : Attribute
    {
        public CodeGeneratorAttribute(string name, string actionMethod)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(nameof(name));
            }

            if(string.IsNullOrWhiteSpace(actionMethod))
            {
                throw new ArgumentException(nameof(actionMethod));
            }

            Name = name;
            ActionMethod = actionMethod;
        }

        /// <summary>
        /// Name of the Code Generator
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Name of the method that needs to be invoked for generating the code.
        /// The method must
        /// a. Be public, non static, non abstract
        /// b  Not accept Generic parameters
        /// c. Have only one argument (the model)
        /// </summary>
        public string ActionMethod { get; private set; }
    }
}
