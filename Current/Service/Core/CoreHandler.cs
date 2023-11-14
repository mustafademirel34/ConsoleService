using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleService.Service.Core.Data.Result;
using ConsoleService.Service.Core.Execution;
using ConsoleService.Service.Core.Interpreter;
using ConsoleService.Service.Editor;

namespace ConsoleService.Service.Core
{
    internal class CoreHandler
    {
        public CoreHandler() { }

        public void RunToDir(string line)
        {

            line = "ConsoleService " + line;

            var mainProcess = new Interpreter.Interpreter();

            var result = mainProcess.Analyze(line);

            ExecuteCommands(result);

            if (result.classType != null)
            {
                if(result.executeName != null)
                    mainProcess.Execute(result);
                else
                {
                    //sınıf var metot yok ?
                }
            }
            else
            {
                Scribe.ErrorText(" <> any class not found");
            }

        }
        // ÇALIŞTIRILMA İŞLEMİNİN VEYA KAYITLARIN GÖRECEĞİ AYARLAMALAR APACAĞI DOSYA

        private void ExecuteCommands(AnalysisResult result)
        {
            foreach (string part in result.commands)
            {

                if (part.Equals($"a"))
                {

                }
                else if (part.Equals($"c"))
                {
                    if (result.classType != null)
                        Constructor.Run(result.classType);
                }
                else if (part.Equals($"b"))
                {

                }
                else
                {

                }
            }

        }
    }




}