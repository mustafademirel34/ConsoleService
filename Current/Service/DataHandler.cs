using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleService.Service
{
    internal class DataHandler
    {
        private static DataHandler instance = null;


        internal DataHandler()//Private olursa kalıtım alınamaz, internal olursa instance aynı proje içerisinde kullanılabilir
        {
            // Yapılandırmadan gelen veriyi kullan
           // DeveloperMode = bool.Parse(configuration["Editor:DeveloperMode"]);

            // Geri kalan veri işlemleri
            ConsoleHistory.AddRange(FileLogger.GetLogData("INPUT").Reverse<string>().Take(9).ToList());


            //var first10Logs = logs.TakeWhile((item, index) => index < 10 && YourCondition(item)).ToList();
            // Verileri başlatmak için gereken kodlar
            //SampleData = "Bu bir örnek veri.";
        }

        internal static DataHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    // IConfiguration nesnesini oluştur
                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();

                    instance = new DataHandler();
                }
                return instance;
            }
        }

        internal string? ConsoleInput = ""; //yoks ahata veriyorrr,, // CI // text.Replace("\r\n", "\n");
        internal List<string> ConsoleHistory = new List<string>();

        /// <summary>
        /// SERVICE BASE
        /// </summary>

        internal string LastCommand = string.Empty;
        internal bool AnyCommand { get { return !string.IsNullOrWhiteSpace(LastCommand); } set { if (!value) LastCommand = string.Empty; } } //LastCommand = value as dynamic;

        internal protected Dictionary<string, Action> TaskActions = new Dictionary<string, Action>();
        internal protected Dictionary<string, Type> TaskFuncs = new Dictionary<string, Type>();

        /// <summary>
        /// GET INPUT
        /// </summary>
        /// 

        /// <summary>
        /// SYSTEM
        /// </summary>
        /// 

        internal bool DeveloperMode = true;

    }
}
