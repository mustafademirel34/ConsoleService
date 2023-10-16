using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleService.ServiceBase.Handler
{
    public class DataHandler
    {
        private static DataHandler instance = null;

        private DataHandler()
        {
            // Verileri başlatmak için gereken kodlar
            //SampleData = "Bu bir örnek veri.";
        }

        public static DataHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataHandler();
                }
                return instance;
            }
        }

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
