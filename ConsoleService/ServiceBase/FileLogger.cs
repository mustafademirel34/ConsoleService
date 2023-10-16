using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleService.ServiceBase
{
    public class FileLogger
    {
        private static string logFilePath = "csp_app_log.txt";

        public static void LogError(string errorMessage)
        {
            using (StreamWriter writer = File.AppendText(logFilePath))
            {
                writer.WriteLine($"{DateTime.Now}: ERROR - {errorMessage}");
            }
        }

        public static void LogUserInput(string userInput)
        {
            using (StreamWriter writer = File.AppendText(logFilePath))
            {
                writer.WriteLine($"{DateTime.Now}: USER INPUT - {userInput}");
            }
        }

        public static void ClearLog()
        {
            File.WriteAllText(logFilePath, string.Empty);
        }

    }
}
