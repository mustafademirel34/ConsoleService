using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;

namespace ConsoleTesting.Core
{
    public class Options
    {


        //public string LastNamespace { get { return Get(""); } set { } } // Prop üzerinden verilebilir

        public Options(string input = "", bool singleCommand = false)
        {
            if (!singleCommand)
            {
                Console.WriteLine();
                SeeAll();
            }

            while (true)
            {
                if (!singleCommand)
                {
                    Utilities.PrintColoredLine("\nConfig > ", ConsoleColor.DarkYellow);
                    input = Console.ReadLine();
                }

                switch (input)
                {
                    case "exit": case "out": case "back": return;
                    case "display": Reverse("TransactionReport"); if (singleCommand) return; else break;
                    case "space":
                        Set("LastNamespace", "");
                        if (singleCommand)
                            return;
                        else
                            break;
                    case "class":
                        Reverse("ClassDetailReport");
                        if (singleCommand)
                            return;
                        else
                            break;
                    case "clear":
                        Utilities.WelcomeScreen();
                        return;
                    case "help":
                        Utilities.PrintHelpInstructions();
                        if (singleCommand)
                            return;
                        else
                            break;
                    case "config":
                        Console.WriteLine();
                        SeeAll();
                        if (singleCommand)
                            return;
                        else
                            break;
                    case "constructor":
                        Reverse("ConstructorPrefence");
                        if (singleCommand)
                            return;
                        else
                            break;
                    case "ad":
                        Set("LastNamespace", "");
                        return;
                    case "change":
                        Utilities.PrintColoredLine("\nConfig > ", ConsoleColor.DarkYellow);
                        Console.Write("Key > ");
                        string k = Console.ReadLine();
                        Utilities.PrintColoredLine("\nConfig > ", ConsoleColor.DarkYellow);
                        Console.Write("Value < ");
                        string v = Console.ReadLine();
                        Set(k,v);
                        return;
                    default:
                        if (singleCommand)
                            return;
                        else
                            break;
                }
            }
        }

        public static bool Getbool(string key)
        {
            return bool.Parse(Get(key));
        }

        public static void Reverse(string key)
        {
            bool value = Getbool(key);
            value = !value;
            Set(key, value);
        }


        public static void SeeAll()
        {
            var allSettings = ConfigurationManager.AppSettings;
            var options = new List<string>() { "constructor", "space, /ad", "display", "class", "Method ParamChar", "Class/Constr ParamChar", "Command Char" };
            int i = 0;
    

            foreach (var key in allSettings.AllKeys)
            {
                string value = allSettings[key];
                value = $"{options[i].ToString()} # {key}: [{value}]";
             
                i++;
            }
            Console.WriteLine("# change # [DANGEROUS]");
        }


        public static string Get(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static void Set(string key, object value)
        {
            // Değerleri okuyun
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var element = config.AppSettings.Settings[key];

            // Anahtarı bulduysanız, değeri güncelleyin ve kaydedin
            if (element != null)
            {
                element.Value = value.ToString();
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            else
            {
                Console.WriteLine($"'{key}' anahtarı bulunamadı.");
            }
        }


    }
}
