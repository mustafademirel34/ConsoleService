using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleService.Service.Editor
{
    public abstract class ActionHandler
    {
        private DataHandler data;
        public abstract void AddAction(); // direkt uygulanması gerekir.
        //public abstract void RemoveAction();

        public ActionHandler()
        {
            data = DataHandler.Instance;
        }

        public void AddAction(string arg, Action action)
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

    }
}
