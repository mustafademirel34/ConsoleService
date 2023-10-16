using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTesting.Test
{
    public class AppScan
    {
        public AppScan(string appName)
        {
            if (IsUygulamaAcik(appName))
                Console.WriteLine("\n>>> " + appName + " is open !");
            else
                Console.WriteLine("\n>>> " + appName + " not open..");

        }

        static bool IsUygulamaAcik(string uygulamaAdi)
        {
            Process[] processes = Process.GetProcessesByName(uygulamaAdi);
            return processes.Length > 0;
        }
    }
}
