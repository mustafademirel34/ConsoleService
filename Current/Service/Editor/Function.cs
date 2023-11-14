using ConsoleService.Service.Core.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleService.Service.Editor
{
    internal class Function
    {
        private static DataHandler Data = DataHandler.Instance;

        protected int AddToHistory(string entry)
        {
            if (!string.IsNullOrWhiteSpace(entry.Trim()))
            {
                if (!Data.ConsoleHistory.Contains(entry, StringComparer.OrdinalIgnoreCase))
                {
                    Data.ConsoleHistory.Insert(0, entry);
                }
            }
            return -1;
        }
        protected void UpdateInputFromHistory(string entry, string tempHistory, int historyIndex, out int cursorLocate)
        {
            ClearCurrentLine();

            if (historyIndex >= 0 && historyIndex < Data.ConsoleHistory.Count)
            {
                Data.ConsoleInput = Data.ConsoleHistory[historyIndex];
                Console.Write(Data.ConsoleInput);
            }
            else if (historyIndex == -1)
            {
                // historyIndex -1 ise en sondaki girişi yazdır ve konumlandır.
                if (Data.ConsoleHistory.Count > 0)
                {
                    Data.ConsoleInput = tempHistory;
                    Console.Write(tempHistory);

                }
                else
                {
                    // History boşsa bir işlem yapabilirsiniz.
                    // Örneğin, boş bir giriş satırı eklemek gibi.
                    Data.ConsoleInput = "";
                    Console.WriteLine("BEN ANLAMADIM");
                }
            }

            //Console.SetCursorPosition(Data.ConsoleInput.Length + 2, Console.CursorTop);
            cursorLocate = Data.ConsoleInput.Length;
        }
        internal void ClearCurrentLine()
        {
            Console.SetCursorPosition(2, Console.CursorTop);
            Console.Write(new string(' ', Data.ConsoleInput.Length + 2)); // Ekstra 2 karakter için (# ve boşluk)
            Console.SetCursorPosition(2, Console.CursorTop);
        }
        internal static string GetMatchingWord(string[] partialWord)
        {
            string returnValue = "";

            FindType findType = new FindType();
            var types = findType.GetTypeAndFolders("ConsoleService");

            //var updatedTypes = new List<string>();
            var updatedTypes = types;
            //foreach (var type in types)
            //{
            //    int firstDotIndex = type.IndexOf('.');
            //    if (firstDotIndex >= 0)
            //    {
            //        string result = type.Substring(firstDotIndex + 1); // +1, ilk "." karakterini atlayarak
            //        updatedTypes.Add(result);
            //    }
            //}

            // Kullanıcıdan gelen metni birleştirin
            string userInput = string.Join(".", partialWord);
            userInput = "ConsoleService." + userInput;

            // Her bir tipi inceleyin ve kullanıcı girdisi ile eşleşen ilk tipi bulun
            string matchedType = updatedTypes.FirstOrDefault(type => type.ToLower().StartsWith(userInput.ToLower(), StringComparison.OrdinalIgnoreCase));
            if (matchedType != null)
            {
                if (matchedType == userInput)
                {
                    // Debug.Write("şşşşşşşşşşşşşşşşşşşşş");
                    List<string> filteredList = updatedTypes
                        .Where(item => item.StartsWith(matchedType.Substring(0, matchedType.LastIndexOf('.'))))
                        .ToList();

                    foreach (var item in filteredList)
                    {
                         Debug.WriteLine(">> "+item);
                    }

                    int index = filteredList.IndexOf(matchedType);

                    if (index != -1)
                    {
                        int nextIndex = (index + 1) % filteredList.Count;
                        string nextType = filteredList[nextIndex];

                        if (nextType != null)
                        {
                            returnValue = nextType;
                        }
                    }
                }
                else
                {
                    returnValue = matchedType;
                }



               

            }
            return returnValue;



        }

    }
}
