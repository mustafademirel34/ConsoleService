using ConsoleService.Service.Core.Data.Result;
using ConsoleService.Service.Core.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleService.Service.Core.Interpreter
{
    internal partial class Interpreter
    {
        public AnalysisResult Analyze(string input)
        {
            AnalysisResult result = new AnalysisResult();

            input = Regex.Replace(input, @"\s+", " ").Trim();

            string[] tokens = input.Split(' ');

            bool isParsingMethod = false;

            foreach (string token in tokens)
            {
                if (token.StartsWith("-") || token.StartsWith("/"))
                {
                    if (token.StartsWith("/"))
                    {
                        result.commands.Add(token.Substring(1));
                    }
                    else if (token.StartsWith("-"))
                    {
                        result.methodParams.Add(token.Substring(1));
                    }
                }
                else if (token.StartsWith("*"))
                {
                    result.classParams.Add(token.Substring(1));
                }
                else
                {
                    result.keywords.Add(token);

                    if (isParsingMethod)
                    {

                        result.executeName = token;
                        isParsingMethod = false;
                    }

                    var classDetect = TypeDetection(string.Join(".", result.keywords));

                    if (classDetect != null)
                    {
                        result.classType = classDetect;
                        result.classes.Add(string.Join(".", result.keywords));
                        isParsingMethod = true;
                    }
                }
            }

           
            if(DataHandler.Instance.DeveloperMode is true)
            {
                //Console.Write("\n"); Console.Write("Giriş: " + input);
                try
                {
                    Console.Write($"\n> [{result.classes[0]}] [{result.executeName}] [{string.Join(",", result.commands)}] [{string.Join(",", result.methodParams)}] [{string.Join(", ", result.classParams)}]");
                }
                catch (Exception)
                {

                }
            }

            /*>>*/
            return result;
        }

        // ALT METHODS

        Type TypeDetection(string path)
        {
            //if (data.DeveloperMode)
            //    Console.WriteLine($"[??> {path}]");
            var classType = Type.GetType(path);
            if (classType != null)
            {
                return classType;
            }

            return null;
        }

        bool MethodDetection(Type classType, string executeName)
        {
            MethodInfo methodInfo = classType.GetMethod(executeName);
            if (methodInfo != null)
            {
                return true;
            }
            return false;
        }

        bool FieldOrPropertieDetection(Type classType, string executeName)
        {
            PropertyInfo propertyInfo = classType.GetProperty(executeName );
            FieldInfo fieldInfo = classType.GetField(executeName);
            if (propertyInfo != null || fieldInfo != null)
            {
                return true;
            }
            return false;
        }
    }
}
