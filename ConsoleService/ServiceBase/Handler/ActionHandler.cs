using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleService.ServiceBase.Main;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleService.ServiceBase.Handler
{
    public abstract class ActionHandler
    {
        private DataHandler data;
        public abstract void AddAction(); // direkt uygulanması gerekir.
        //public abstract void RemoveAction();

        public ActionHandler()
        {
            data = DataHandler.Instance;
            //RegisterAction("read", );

        }

        public void RegisterAction(string arg, Action action)
        {
            data.TaskActions[arg] = action;
        }

        public void RunAction(string arg)
        {
            if (data.AnyCommand)
            {
                Console.WriteLine("The command: [" + data.LastCommand + "] executed.");
                data.AnyCommand = false;
                data.TaskActions[arg].Invoke();
            }
        }

        public void RunAction(string action, string command)
        {
            if (GetAction(action))
            {
                RunAction(command);
            }
        }

        public bool GetAction(string arg)
        {
            if (data.TaskActions.ContainsKey(arg))
            {
                data.LastCommand = arg;
            }
            else
            {
                data.LastCommand = string.Empty;
            }

            return data.AnyCommand;
        }
        public void TakeInput()
        {
            UserInput get = new UserInput();
        }


    }
}
