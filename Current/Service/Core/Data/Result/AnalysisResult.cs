using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleService.Service.Core.Data.Result
{
    internal class AnalysisResult : Result
    {
        internal List<string> keywords = new List<string>();
        internal List<string> classes = new List<string>();
        internal List<string> commands = new List<string>();
        internal List<string> methodParams = new List<string>();
        internal List<string> classParams = new List<string>();

        internal Type classType;
        internal string executeName;
    }
}
