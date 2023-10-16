using ConsoleService.ServiceBase.Handler;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleService.ServiceBase.Main
{
    public class Interpreter
    {
        DataHandler data;
        public Interpreter()
        {
            data = DataHandler.Instance;
        }

        public void Analyze(string input)
        {
            string[] tokens = input.Split(' ');

            List<string> keywords = new List<string>();
            List<string> classes = new List<string>();
            List<string> commands = new List<string>();
            List<string> methodParams = new List<string>();
            List<string> classParams = new List<string>();

            bool isParsingMethod = false;
            string methodName = "";

            foreach (string token in tokens)
            {
                if (token.StartsWith("-") || token.StartsWith("/"))
                {
                    if (token.StartsWith("/"))
                    {
                        commands.Add(token.Substring(1));
                    }
                    else if (token.StartsWith("-"))
                    {
                        methodParams.Add(token.Substring(1));
                    }
                }
                else if (token.StartsWith("*"))
                {
                    classParams.Add(token.Substring(1));
                }
                else
                {
                    keywords.Add(token);

                    if (isParsingMethod)
                    {
                        methodName = token;
                        isParsingMethod = false;
                    }

                    if (TypeDetection(string.Join(".", keywords)) != null)
                    {
                        classes.Add(string.Join(".", keywords));
                        isParsingMethod = true;
                    }
                }
            }

            if (data.DeveloperMode)
            {
                Console.WriteLine("Giriş: " + input);
                Console.WriteLine("Anahtar Kelimeler: " + string.Join(", ", keywords));
                Console.WriteLine("Sınıflar: " + string.Join(", ", classes));
                Console.WriteLine("Metot Adı: " + methodName);
                Console.WriteLine("Komutlar: " + string.Join(", ", commands));
                Console.WriteLine("Metot Parametreleri: " + string.Join(", ", methodParams));
                Console.WriteLine("Sınıf Parametreleri: " + string.Join(", ", classParams));
            }
        }

        Type TypeDetection(string path)
        {
            if (data.DeveloperMode)
                Console.WriteLine($"[?c> {path}]");

            var classType = Type.GetType(path);
            if (classType != null)
            {
                gengeltp = classType;
                return classType;
            }

            return null;
        }

        public Type gengeltp;

    }
}
