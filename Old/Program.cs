using System;
using System.Text;

class Program
{
    static string userInput = "";
    static void DeleteFromScreen(int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            if (userInput.Length > 0)
            {
                userInput = userInput.Substring(0, userInput.Length - 1); // Son harfi sil.
                Console.Write("\b \b"); // Silinen karakteri ekrandan temizle.
            }
        }

    }
    static void Main()
    {

        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Tuş takibini başlatmak için ENTER tuşuna basın. Çıkmak için 'ESC' tuşuna basın.");
        List<string> kelimeler = new List<string> { "Demirel", "Deneme", "Deneyim", "Denge" };

        while (true)
        {
            ConsoleKeyInfo tus = Console.ReadKey(true);

            if (tus.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
            }
            if (tus.Key == ConsoleKey.Tab)
            {
                string[] sentences = userInput.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var sonKelime = sentences[sentences.Length - 1];

                string eslesenKelime = kelimeler.FirstOrDefault(k => k.StartsWith(sonKelime, StringComparison.OrdinalIgnoreCase));

                if (eslesenKelime != null)//Bir eşleşme bulunursa gerçekleşecek
                {
                    //************************************************************************************
                    //int indeks = eslesenKelime.IndexOf(sonKelime, StringComparison.OrdinalIgnoreCase);
                    //string geriKalanKisim = eslesenKelime.Substring(indeks + sonKelime.Length);
                    //Sadece yazının geri kalanını göstermek istersen DeleteFromScreen kaldırılır ve ekrana geriKalanKisim yazdırılır.
                    DeleteFromScreen(sonKelime.Length);
                    userInput += eslesenKelime; // Yeni karakteri ifadeye ekle.
                    Console.Write(eslesenKelime); // Ekrana yazdır.
                }
            }
            else if (tus.Key == ConsoleKey.Backspace)
            {
                if (userInput.Length > 0)
                {
                    DeleteFromScreen();
                }
            }
            else if (tus.Key == ConsoleKey.Escape)
            {
                break;
            }
            else
            {
                userInput += tus.KeyChar;
                Console.Write(tus.KeyChar);
            }
        }

        Console.WriteLine("Program sonlandı.");
    }
}
