using ConsoleTesting.Helper;
using ConsoleTesting.Project.Core;
using ConsoleTesting.Test;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleTesting.Core
{

    public class Processor
    {
        private string NoFlour, NoNamespace;
        private List<object> LoMParameters;
        private List<object> LoCommands;
        private List<object> LoCParameters;

        public Processor()
        {
            LoCParameters = new List<object>();
            LoMParameters = new List<object>();
            LoCommands = new List<object>();
            NoFlour = string.Empty;
            NoNamespace = string.Empty;
        }

        public void Analyze(string input)
        {
            string[] inputParts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            //////////////// SHIELD

            foreach (string part in inputParts)
            {

                if (part.StartsWith(Options.Get("CharPOM")))
                {
                    LoMParameters.Add(part.Replace(Options.Get("CharPOM"), null));
                    inputParts = Utilities.RemoveValueFromArray(inputParts, part);
                }
            }

            foreach (string part in inputParts)
            {
                if (part.StartsWith(Options.Get("CharOC")))
                {
                    LoCommands.Add(part);
                    //LoMParameters.Add(part.Replace(Configuration.Get("CharOC"), null));
                    inputParts = Utilities.RemoveValueFromArray(inputParts, part);
                }
            }

            foreach (string part in inputParts)
            {
                if (part.StartsWith(Options.Get("CharPOC")))
                {
                    LoCParameters.Add(part.Replace(Options.Get("CharPOC"), null));
                    inputParts = Utilities.RemoveValueFromArray(inputParts, part);
                }
            }

            /////////////////////////////////
            ///

            string last = Options.Get("LastNamespace");

            // SENARYOLAR

            if (!string.IsNullOrWhiteSpace(last)) //DOLUYSA
            {
                NoNamespace = last;
                if (inputParts.Length == 0)
                {
                    Console.WriteLine("Hata: Sadece Class Özel Komut");
                }
                else if (inputParts.Length == 1)
                {
                    if (last.Substring(last.LastIndexOf('.') + 1) != inputParts[0])
                        NoFlour = inputParts[0];
                }
                else
                {
                    Console.WriteLine("GERİYE KLANALAR DİKKATE ALINAMAZ");
                }
            }
            else
            {
                if (inputParts.Count() > 0)
                {
                    if (inputParts[0] != null) // içerik var
                    {
                        //NoNamespace = inputParts[0];

                        foreach (string part in inputParts)
                        {
                            NoNamespace = NoNamespace + part + ".";
                        }

                        //int lastDotIndex = input.LastIndexOf(".");

                        NoNamespace = string.Format("{0}.{1}", nameof(ConsoleTesting), NoNamespace.Remove(NoNamespace.Length - 1));
                        // Console.WriteLine(NoNamespace);
                    }
                }
                else
                {
                    Console.WriteLine("Tek komut");
                }
            }


            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            if (Options.Getbool("TransactionReport"))
                Console.WriteLine("\nLastInput: " + last + "\nFSpace: " + NoNamespace + "\nMethod: " + NoFlour); // GNamespace, NoClass


            OperationOnClass();

        }


        void OperationOnClass()
        {
            var classType = TypeDetection();
            if (classType != null)
            {
                //if(classType.Name == NoNamespace.Substring(NoNamespace.LastIndexOf(".") + 1))
                //{
                //    ListMethodsAndProperties(classType);
                //}
                //else
                {
                    var command = ExecuteCommands(classType);
                    /**/
                    if (command && string.IsNullOrWhiteSpace(Options.Get("LastNamespace"))) //temel alındığında yine de metotları ekrana yazdırma sorun uvardı, uraya taşıtık,
                        ListMethodsAndProperties(classType);

                    if (!string.IsNullOrWhiteSpace(NoFlour))
                        ExecuteProcess();
                }
            }
            else
            {
                //SINIF BULUNAMAZSA İŞLEMLER
                {

                    //var result = GetClassesAndFolders.GetItemsFromPath(NoNamespace, "class");

                    var result = default(List<string>);

                    if (result.Count() >= 1)
                    {
                        foreach (var item in LoCommands)
                        {
                            var x = Options.Get("CharOC");
                            Console.Write("\nClass: ");
                            if (item.Equals($"{x}b"))
                            {
                                foreach (var c in result)
                                {
                                    Console.Write($"{c}, ");
                                }
                                Console.WriteLine();
                            }
                        }
                    }
                    else
                    {
                        Utilities.PrintColoredLine("Class not found", ConsoleColor.Red, true);
                    }
                }
            }
        }

        bool ExecuteCommands(Type classType)
        {
            var x = Options.Get("CharOC");
            bool showc = true;
            foreach (string part in LoCommands)
            {

                if (part.Equals($"{x}a"))
                {
                    Options.Set("LastNamespace", NoNamespace); // ??????????????????????
                    showc = false;
                }
                else if (part.Equals($"{x}c"))
                {
                    var execution = new Execution();
                    execution.RunConstructor(classType);
                    showc = false;
                }
                else if (part.Equals($"{x}b"))
                {

                }
                else
                {
                    NoFlour = part.Replace(x, "");
                    showc = false;
                }
            }
            return showc;
        }

        Type TypeDetection()
        {
            var classType = Type.GetType(NoNamespace);
            if (classType != null)
            {
                return classType;
            }

            return null;

        }


        void ExecuteProcess()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type classType = assembly.GetType(NoNamespace);
            Execution execution = new Execution();

            bool isMethod = MethodDetection(NoFlour, classType);
            bool isPropField = FieldOrPropertieDetection(NoFlour, classType);

            if (isPropField && !isMethod)
            {
                if (Options.Getbool("TransactionReport"))
                    Utilities.PrintColoredLine("\nField or Prop has been verified\n", ConsoleColor.DarkGreen, true);

                var result = execution.ReadPropertyOrField(NoNamespace, NoFlour);

                if (result != null)
                    Utilities.PrintColoredLine("> " + result + "\n", ConsoleColor.Cyan);
                else
                    Utilities.PrintColoredLine($"{NoFlour} is null\n", ConsoleColor.Red);
            }
            else if (!isPropField && isMethod)
            {
                if (Options.Getbool("TransactionReport"))
                    Utilities.PrintColoredLine("\nMethod has been verified\n", ConsoleColor.DarkGreen, true);

                dynamic[] parametersArray1 = LoMParameters.ToArray();
                dynamic[] parametersArray2 = LoCParameters.ToArray();



                string result = execution.RunMethod(NoNamespace, NoFlour, parametersArray1, parametersArray2).ToString();

                if (result != null)
                {
                    ConsoleColor consoleColor;

                    if (result[0].ToString() == "X")
                    {
                        consoleColor = ConsoleColor.Red;
                        result = (string)result.Substring(1);
                    }
                    else
                        consoleColor = ConsoleColor.Cyan;

                    Utilities.PrintColoredLine(result + "\n", consoleColor);
                }
            }
            else
            {
                //  Console.WriteLine("hiçbiri");
                string lastPart = NoNamespace.Substring(NoNamespace.LastIndexOf('.') + 1).Replace(".", "");
                if (lastPart != NoFlour)
                    Utilities.PrintColoredLine("Any method, field, or property was not found", ConsoleColor.Red, true);
                //Else
                //ListMethodsAndProperties(classType);
            }
        }

        void ListMethodsAndProperties(Type type)
        {
            var methods = type.GetMethods();

            Console.Write("\nContents: ");
            int hiddenCount = 0;

            if (Options.Getbool("ClassDetailReport")) // true ise her şeyi göster
            {
                foreach (var method in methods)
                {
                    Console.Write($"{method.Name}, ");
                }
            }
            else // false ise belirli metotları gösterme
            {
                foreach (var method in methods)
                {
                    if (!Utilities.ContainsAny(method.Name, "get_", "set_", "GetType", "ToString", "GetHashCode", "Equals"))
                    {
                        Console.Write($"{method.Name}, ");
                    }
                    else
                    {
                        hiddenCount++;
                    }
                }
            }

            if (hiddenCount > 0)
            {
                Console.Write($"Hidden({hiddenCount}), ");
            }

            // Public özellikleri listele
            foreach (PropertyInfo property in type.GetProperties())
            {
                Console.Write($"*{property.Name}, ");
            }

            foreach (var field in type.GetFields())
            {
                Console.Write($"*{field.Name}, ");
            }

            Console.WriteLine();
        }

        bool MethodDetection(string userInput, Type classType)
        {
            MethodInfo methodInfo = classType.GetMethod(userInput);
            if (methodInfo != null)
            {
                return true;
            }
            return false;
        }

        bool FieldOrPropertieDetection(string userInput, Type classType)
        {
            PropertyInfo propertyInfo = classType.GetProperty(userInput);
            FieldInfo fieldInfo = classType.GetField(userInput);
            if (propertyInfo != null || fieldInfo != null)
            {
                return true;
            }
            return false;
        }




    }

}
