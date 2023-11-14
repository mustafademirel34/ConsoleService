using ConsoleService.Service.Core.Data.Result;
using ConsoleService.Service.Core.Execution;
using ConsoleService.Service.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleService.Service.Core.Interpreter
{
    internal partial class Interpreter
    {
        internal void Execute(AnalysisResult result) // Tüm işlemleri yöneten sevgili metotumuz
        {
            //Assembly assembly = Assembly.GetExecutingAssembly();
            //Type classType = assembly.GetType(NoNamespace);

            bool isMethod = MethodDetection(result.classType, result.executeName);

            bool isPropField = FieldOrPropertieDetection(result.classType, result.executeName);

            if (isPropField && !isMethod)
            {
                if (DataHandler.Instance.DeveloperMode)
                    Scribe.SuccessText("\nField or Prop has been verified");

                var returnValue = ReadValue.Run(result.executeName, result.classType);

                if (returnValue != null)
                    Scribe.OutputText($"<>{returnValue}");
                else
                    Scribe.ErrorText($" <>{returnValue} was null");
            }
            else if (!isPropField && isMethod)
            {
                if (DataHandler.Instance.DeveloperMode)
                    Scribe.SuccessText("\nMethod has been verified");

                dynamic[] parametersArray1 = result.methodParams.ToArray();
                dynamic[] parametersArray2 = result.classParams.ToArray();
                var method = result.classType.GetMethod(result.executeName);

                //ctorpreference

                var returnValue = Method.Run(result.classType, method, parametersArray1, parametersArray2, false);

                if (returnValue != null)
                {
                    
                    if (!returnValue.Success)
                    {
                        Scribe.ErrorText(" <> "+returnValue.Message);
                        return;
                    }
                   
                        Scribe.OutputText(" <> " + returnValue.Value);
                }
            }
            else
            {
                Scribe.ErrorText("Any method, field, or property was not found");
                ////  Console.WriteLine("hiçbiri");
                //string lastPart = NoNamespace.Substring(NoNamespace.LastIndexOf('.') + 1).Replace(".", "");
                //if (lastPart != NoFlour)
                //    Utilities.PrintColoredLine("Any method, field, or property was not found", ConsoleColor.Red, true);
                ////Else
                ////ListMethodsAndProperties(classType);
            }
        }
    }
}
