using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleTesting.Project.Core
{
    internal class FilesAndFolders
    {
        internal static void GetListFilesAndFolders(string dizin, List<string> klasorListesi, List<string> sinifListesi)
        {
            // Dizin içindeki dosyaları ve klasörleri alın.
            string[] dosyalar = Directory.GetFiles(dizin);
            string[] klasorler = Directory.GetDirectories(dizin);

            foreach (string dosya in dosyalar)
            {
                // Sadece .cs uzantılı dosyaları sınıf olarak kabul edin.
                if (dosya.EndsWith(".cs"))
                {
                    string icerik = File.ReadAllText(dosya);
                    string namespaceRegex = @"namespace\s+([^\s;]+)";

                    // Dosya içeriğinde namespace'i kontrol edin.
                    Match match = Regex.Match(icerik, namespaceRegex);
                    if (match.Success)
                    {
                        string namespaceAdi = match.Groups[1].Value;
                        sinifListesi.Add(namespaceAdi + "." + Path.GetFileNameWithoutExtension(dosya));
                    }
                }
            }

            foreach (string klasor in klasorler)
            {
                klasorListesi.Add(Path.GetFileName(klasor));
            }
        }
    }
}
