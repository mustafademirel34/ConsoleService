using ConsoleService;
using ConsoleService.ServiceBase.Main;
using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        //ServiceHandler serviceHandler = new ServiceHandler();

        //var test = serviceHandler.GetAction("read");

        //if (test)
        //{
        //    serviceHandler.RunAction("read");
        //}

        //serviceHandler.TakeInput();

        Interpreter interpreter = new Interpreter();
        interpreter.Analyze("ConsoleService ServiceBase Main UserInput Anam babam *50 -45 /c");

        Console.WriteLine(interpreter.gengeltp);

        //Console.ReadLine();

        //if (result)
        //{

        //}
    }
}