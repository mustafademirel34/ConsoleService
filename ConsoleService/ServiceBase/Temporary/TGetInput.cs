using ConsoleService.ServiceBase.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleService.ServiceBase.Temporary
{
    internal class TGetInput : UserInput
    {
        private string SearchInHistory(string searchTerm)
        {
            currentUserInput = "";
            // Geçmiş girdileri ara ve eşleşenleri döndür
            var matchingEntries = history.Where(entry => entry.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            if (matchingEntries.Count > 0)
            {
                // Eşleşen girdileri kullanıcıya göster veya işlem yap
                var result = "Eşleşen girdiler:\n";
                result += string.Join("\n", matchingEntries);
                return result;
            }
            else
            {
                return "Eşleşen girdi bulunamadı.";
            }
        }
        private bool ValidateInput(string input)
        {
            // Gelen girdiyi istediğiniz şekilde doğrulayın
            if (!string.IsNullOrWhiteSpace(input))
            {
                // Örnek: Girdinin uzunluğu 3'ten fazla olmalıdır
                return input.Length > 3;
            }
            return false;
        }
        private void SaveInputToHistory(string input)
        {
            // Gelen girdiyi bir veritabanına, dosyaya veya geçici bir veri yapısına kaydet
            // Bu, kullanıcıların daha sonra bu geçmiş girdileri görmelerine veya geri yüklemelerine yardımcı olabilir
            if (ValidateInput(input))
            {
                history.Add(input);
                // Örnek: Geçmiş girdi başarıyla kaydedildi
                LogUserActions("Geçmişe kaydedildi: " + input);
            }
        }
        private List<string> GetAutoCompletions(string input)
        {
            // Kullanıcının girdisine dayalı önerilen tamamlamaları hesaplayın
            var completions = new List<string>();
            foreach (var word in kelimeler)
            {
                if (word.StartsWith(input, StringComparison.OrdinalIgnoreCase))
                {
                    completions.Add(word);
                }
            }
            return completions;
        }
        private string ProcessInput(string input)
        {
            // Kullanıcının girdisini işlemek için özel işlevler ekleyin
            // Örnek işlem: Girdiyi büyük harfe dönüştür
            return input.ToUpper();
        }
        private bool CheckSyntax(string input)
        {
            // Kullanıcının girdisini belirli bir söz dizimine uygunluğunu kontrol edin
            // Örnek: Girdi geçerli bir komut ise true döndür
            if (input.StartsWith("komut", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
        private void LogUserActions(string action)
        {
            // Kullanıcının yaptığı işlemleri bir işlem günlüğüne kaydet
            // Örnek: İşlem günlüğüne kaydet
            Console.WriteLine("İşlem Günlüğü: " + action);
        }
        private void HandleCustomShortcuts(ConsoleKeyInfo keyInfo)
        {
            // Belirli tuş kombinasyonlarına (kısayollar) özel işlevler ekleyin
            if (keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control) && keyInfo.Key == ConsoleKey.S)
            {
                // Örnek: Ctrl + S tuşlarına kaydetme işlemi ekleyin
                SaveInputToHistory(currentUserInput);
            }
        }


    }
}
