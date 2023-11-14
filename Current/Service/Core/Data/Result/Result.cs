using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleService.Service.Core.Data.Result
{
    internal class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public dynamic Value { get; set; }
    }
}
