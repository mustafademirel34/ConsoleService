using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleService.Service
{
    public class FileLogger
    {
        private static string logFilePath = Directory.GetCurrentDirectory() + "/csplogs.txt";

        public static void LogError(string errorMessage)
        {
            using (StreamWriter writer = File.AppendText(logFilePath))
            {
                writer.WriteLine($"[{DateTime.Now}] ERROR > {errorMessage}");
            }
        }

        public static void LogUserInput(string userInput)
        {
            using (StreamWriter writer = File.AppendText(logFilePath))
            {
                writer.WriteLine($"[{DateTime.Now}] INPUT > {userInput}");
            }
        }

        public static void LogOutput(string userInput)
        {
            using (StreamWriter writer = File.AppendText(logFilePath))
            {
                writer.WriteLine($"[{DateTime.Now}] OUTPUT > {userInput}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log">INPUT, ERROR, OUTPUT</param>
        /// <returns></returns>
        public static List<string> GetLogData(string log)
        {
            List<string> values = new List<string>();
            using (StreamReader reader = new StreamReader(logFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Log satırında "INPUT" geçiyorsa, bu satırı işle ve değeri al
                    if (line.Contains(log))
                    {
                        // Log satırını düzenli ifade ile ayrıştır
                        Match match = Regex.Match(line, @"\[([^]]*)\] " + log + " > (.+)");
                        if (match.Success)
                        {
                            string timestamp = match.Groups[1].Value;
                            string inputValue = match.Groups[2].Value;
                            values.Add(inputValue);
                        }
                    }
                }
            }

            return values;
        }

        public static void ClearLog()
        {
            File.WriteAllText(logFilePath, string.Empty);
        }

    }
}
