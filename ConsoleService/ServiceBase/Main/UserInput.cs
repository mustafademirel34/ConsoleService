using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleService.ServiceBase.Main
{
    internal class UserInput
    {
        // TGetInput'da bazı denemeler gerçekleştiriliyor. Bu yüzden protected olmalıdır.

        protected readonly List<string> history = new List<string>();
        protected int historyIndex = -1;
        protected new static string currentUserInput = "";
        protected string temporaryHistoryData = string.Empty;

        protected static List<string> kelimeler = new List<string>
        {
            "gameplay",
            "gamer",
            "gameshow",
            "gamification",
            "gamble",
            "gamechanger",
            "gamepad",
            "gamestop",
            "gametes",
            "gamester"
            // Diğer kelime önerilerini buraya ekleyebilirsiniz.
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="multiple">Birden fazla komut girmek için true işaretleyin. Durum false ise, bir komut çalıştırılacaktır.</param>

        public UserInput(bool multiple = false)
        {
            bool newline = true;

            do
            {
                if (newline)
                {
                    //if(!history.Any())
                    Console.Write("# ");
                    newline = false;
                }

                var keyInfo = Console.ReadKey(intercept: true);

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    HandleEnterKey(ref newline, multiple);
                    if (!multiple)
                    {
                        break; // multiple = false ise Enter'a basıldığında döngüyü durdur
                    }
                }
                else
                {
                    HandleKeyPress(keyInfo, ref newline);
                }

            } while (true);
        }

        private void HandleKeyPress(ConsoleKeyInfo keyInfo, ref bool newline)
        {
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                HandleEnterKey(ref newline, true);
            }
            else if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                HandleUpArrow();
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                HandleDownArrow();
            }
            else if (keyInfo.Key == ConsoleKey.Backspace)
            {
                HandleBackspace();
            }
            else if (keyInfo.Key == ConsoleKey.Delete)
            {
                HandleDeleteKey();
            }
            else if (keyInfo.Key == ConsoleKey.Escape)
            {
                Environment.Exit(0);
            }
            else if (keyInfo.Key == ConsoleKey.Tab)
            {
                HandleTabKey();
            }

            else if (keyInfo.Key == ConsoleKey.RightArrow)
            {
                // Sağa ok tuşuna tıklandığında imleci bir karakter sağa kaydır.
                if (Console.CursorLeft < Console.WindowWidth - 1)
                {
                    Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                }
            }
            else if (keyInfo.Key == ConsoleKey.LeftArrow)
            {
                // Sola ok tuşuna tıklandığında imleci bir karakter sola kaydır.
                if (Console.CursorLeft > 2)
                {
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                }
            }

            else
            {
                HandleNormalKey(keyInfo.KeyChar);
            }
        }

        private void HandleDeleteKey()
        {
            if (Console.CursorLeft < currentUserInput.Length + 2)
            {
                int left = Console.CursorLeft;
                int top = Console.CursorTop;

                if (currentUserInput.Length > left - 2)
                {
                    currentUserInput = currentUserInput.Remove(left - 2, 1);
                    Console.SetCursorPosition(2, top);
                    Console.Write(currentUserInput + new string(' ', currentUserInput.Length - left + 2));
                    Console.SetCursorPosition(left, top);
                }
            }
        }

        private void HandleEnterKey(ref bool newline, bool multiple)
        {
            FileLogger.LogUserInput(currentUserInput);

            AddToHistory(currentUserInput);

            currentUserInput = "";
            newline = true;

            if (multiple)
                Console.WriteLine();
        }


        private void HandleUpArrow()
        {
            if (history.Count == 0) return;

            if (historyIndex == -1)
            {
                if (!string.IsNullOrWhiteSpace(currentUserInput) && history[0] != currentUserInput)
                {
                    temporaryHistoryData = currentUserInput;
                }
            }

            if (historyIndex == -1)
            {
                historyIndex = 0;
            }
            else
            {
                historyIndex = Math.Min(historyIndex + 1, history.Count - 1);
            }

            UpdateInputFromHistory();
        }

        private void HandleDownArrow()
        {
            //if (history.Count == 0) return;

            if (historyIndex >= 0)
            {
                historyIndex = Math.Max(historyIndex - 1, -1);
            }
            //if (historyIndex == -1)
            //{
            //    historyIndex = 0;
            //}


            UpdateInputFromHistory();
        }

        private void AddToHistory(string entry)
        {
            if (!string.IsNullOrWhiteSpace(entry.Trim()))
            {
                if (!history.Contains(entry, StringComparer.OrdinalIgnoreCase))
                {
                    history.Insert(0, entry);
                }
                historyIndex = -1;
            }
        }


        private void HandleTabKey()
        {
            string[] sentences = currentUserInput.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var lastWord = "";
            if (!string.IsNullOrWhiteSpace(currentUserInput))
            {
                lastWord = sentences.Last();

                string matchingWord = GetMatchingWord(lastWord);

                if (matchingWord != null)
                {
                    HandleBackspaceOld(lastWord.Length);
                    currentUserInput += matchingWord;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(matchingWord);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

        }

        private void HandleNormalKey(char key)
        {
            currentUserInput += key;
            Console.Write(key);
        }

        private void UpdateInputFromHistory()
        {
            ClearCurrentLine(currentUserInput);

            if (historyIndex >= 0 && historyIndex < history.Count)
            {
                currentUserInput = history[historyIndex];
                Console.Write(currentUserInput);
            }
            else if (historyIndex == -1)
            {
                // historyIndex -1 ise en sondaki girişi yazdır ve konumlandır.
                if (history.Count > 0)
                {
                    currentUserInput = temporaryHistoryData;
                    Console.Write(temporaryHistoryData);
                }
                else
                {
                    // History boşsa bir işlem yapabilirsiniz.
                    // Örneğin, boş bir giriş satırı eklemek gibi.
                    currentUserInput = "";
                    Console.WriteLine("BEN ANLAMADIM AQ");
                }
            }
        }

        private static void ClearCurrentLine(string text)
        {
            Console.SetCursorPosition(2, Console.CursorTop);
            Console.Write(new string(' ', text.Length + 2)); // Ekstra 2 karakter için (# ve boşluk)
            Console.SetCursorPosition(2, Console.CursorTop);
        }

        private string GetMatchingWord(string partialWord)
        {
            return kelimeler.FirstOrDefault(k => k.StartsWith(partialWord, StringComparison.OrdinalIgnoreCase));
        }

        private void HandleBackspaceOld(int count = 1) // Old
        {
            for (int i = 0; i < count; i++)
            {
                if (currentUserInput.Length > 0)
                {
                    currentUserInput = currentUserInput.Substring(0, currentUserInput.Length - 1);
                    Console.Write("\b \b");
                }
                else
                {
                    break;
                }
            }
        }
        private void HandleBackspace()
        {
            if (currentUserInput.Length > 0)
            {
                int left = Console.CursorLeft;
                int top = Console.CursorTop;
                int currentLength = currentUserInput.Length;

                if (left > 2)
                {
                    int endOfInput = left - 3;
                    if (endOfInput >= 0)
                    {
                        currentUserInput = currentUserInput.Remove(endOfInput, 1);
                        Console.SetCursorPosition(2, top);
                        Console.Write(currentUserInput + new string(' ', currentLength - currentUserInput.Length) + " ");
                        Console.SetCursorPosition(left - 1, top);
                    }
                }
            }
        }


    }
}
