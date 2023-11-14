using ConsoleService.Service.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConsoleService.Service.Editor;

namespace ConsoleService.Service
{
    public class ServiceHandler
    {
        private DataHandler dataHandler;
        private CoreHandler coreHandler;
        public ServiceHandler()
        {
            dataHandler = new DataHandler();
            coreHandler = new CoreHandler();
        }

        // KULLANICININ GÖRECEĞİ AYARLAMALAR APACAĞI DOSYA

        public void ExecuteDir(string line)
        {
            coreHandler.RunToDir(line);
        }

        public void ConsoleRead()
        {
            Read read = new Read(false);
        }

    }
}
