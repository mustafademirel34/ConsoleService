using ConsoleService.Service;
using ConsoleService.Service.Core.Data;
using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        ServiceHandler serviceHandler = new ServiceHandler();

        // serviceHandler.ExecuteDir("ConsoleService " + Console.ReadLine().Trim());

        // serviceHandler.ConsoleRead();



        // Ayarı silme
        // ConfigurationManagerHelper.RemoveSetting("MySettingKey");

        //FindType findType = new FindType(); // Create an instance of the FindType class
        //string targetNamespace = ""; // Replace with the actual namespace you want to search for
        //List<string> classList = findType.GetTypeAndFolders(targetNamespace);

        //if (classList != null)
        //{
        //    foreach (string className in classList)
        //    {
        //        Console.WriteLine(className);
        //    }
        //}
        //else
        //{
        //    Console.WriteLine("Project root directory not found.");
        //}






        //string dosyaYolu = Directory.GetCurrentDirectory() + "/csp_log.txt";
        //if (File.Exists(dosyaYolu))
        //{
        //    // Metin dosyasını varsayılan metin düzenleyici ile aç
        //    ProcessStartInfo psi = new ProcessStartInfo
        //    {
        //        FileName = "notepad.exe", // Notepad uygulamasını kullanabilirsiniz
        //        Arguments = dosyaYolu
        //    };

        //    using (Process process = new Process { StartInfo = psi })
        //    {
        //        process.Start();
        //        //process.WaitForExit();
        //    }
        //}


    }

}