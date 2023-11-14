using ConsoleService.Service.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleService.Service.Editor
{
    internal class Operator : Function
    {
        internal static string LineInput
        {
            get
            {
                if (DataHandler.Instance.ConsoleInput is string ci && ci is not null)
                {
                    return ci;
                }
                else
                    return "";
            }
            set
            {
                DataHandler.Instance.ConsoleInput = value;
            }
        }

        internal static List<string> ConsoleHistory
        {
            get
            {
                if (DataHandler.Instance.ConsoleHistory is List<string> hi && hi is not null)
                {
                    return hi;
                }
                throw new Exception("KeyOperator h"); //?
            }
            set
            {
                DataHandler.Instance.ConsoleHistory = value;
            }
        }

        // ****************************************************************

        protected int historyIndex = -1;
        protected string temporaryHistoryData = string.Empty;
        protected static int cursorLeftLocate;

        // ****************************************************************

        internal void HandleDeleteKey()
        {
            if (Console.CursorLeft < LineInput.Length + 2)
            {

                int left = Console.CursorLeft;
                int top = Console.CursorTop;
                if (LineInput.Length > left - 2)
                {
                    //Console.Beep();
                    LineInput = LineInput.Remove(left - 2, 1);
                    Console.SetCursorPosition(2, top);
                    Console.Write(LineInput + new string(' ', LineInput.Length - left + 3));//3 yaptık?
                    Console.SetCursorPosition(left, top);
                }
            }
        }
        internal void HandleEnterKey(bool multiLineMode)
        {
            FileLogger.LogUserInput(LineInput);

            CoreHandler coreHandler = new CoreHandler();
            coreHandler.RunToDir(LineInput);

            historyIndex = AddToHistory(LineInput);

            LineInput = "";

            if (multiLineMode)
                Console.WriteLine();

            cursorLeftLocate = 0;
        }
        internal void HandleUpArrow()
        {
            if (ConsoleHistory.Count == 0) return;

            if (historyIndex == -1)
            {
                if (!string.IsNullOrWhiteSpace(LineInput) && ConsoleHistory[0] != LineInput)
                {
                    temporaryHistoryData = LineInput;
                }
            }

            if (historyIndex == -1)
            {
                historyIndex = 0;
            }
            else
            {
                historyIndex = Math.Min(historyIndex + 1, ConsoleHistory.Count - 1);
            }

            UpdateInputFromHistory("", temporaryHistoryData, historyIndex, out cursorLeftLocate);
        }
        internal void HandleDownArrow()
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

            UpdateInputFromHistory(LineInput, temporaryHistoryData, historyIndex, out cursorLeftLocate);

            //yaazzz
        }
        internal void HandleLeftArrowKey()
        {
            // Sola ok tuşuna tıklandığında imleci bir karakter sola kaydır.
            if (Console.CursorLeft > 2)
            {
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                cursorLeftLocate--;
            }
        }
        internal void HandleRightArrowKey()
        {
            if (cursorLeftLocate < LineInput.Length)
            {
                // Sağa ok tuşuna tıklandığında imleci bir karakter sağa kaydır.
                if (Console.CursorLeft < Console.WindowWidth - 1)
                {
                    Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                    cursorLeftLocate++;
                }
            }
        }
        internal void HandleTabKey()
        {

            string[] sentences = LineInput.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var lastWord = "";

            if (!string.IsNullOrWhiteSpace(LineInput) && LineInput.Last().ToString() != " ")
            {
                lastWord = sentences.Last();

                string matchingWord = GetMatchingWord(sentences);

                //Debug.WriteLine("MATCH>" + matchingWord + "< EDITED> "+ matchingWord.Substring(matchingWord.LastIndexOf('.') + 1));
                //Debug.WriteLine("LAST>" + lastWord + "<");

                if (!string.IsNullOrWhiteSpace(matchingWord))
                {
                    int lastDotIndex = matchingWord.LastIndexOf('.'); // Son "." karakterinin indeksini bulur
                    if (lastDotIndex >= 0)
                    {
                        string desiredSubstring = matchingWord.Substring(lastDotIndex + 1); // Son "." karakterinden sonraki kısmı alır

                        HandleBackspaceOld(lastWord.Length);
                        LineInput += desiredSubstring;

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(desiredSubstring);
                        Console.ForegroundColor = ConsoleColor.White;

                        cursorLeftLocate = LineInput.Length;
                    }



                }
            }



        }

        internal void HandleNormalKey(char key)
        {
            try
            {
                if (cursorLeftLocate >= 0 && cursorLeftLocate <= LineInput.Length)
                {
                    LineInput = LineInput.Insert(cursorLeftLocate, key.ToString());
                    cursorLeftLocate++;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e + "> " + LineInput + " > " + cursorLeftLocate);
            }


            ClearCurrentLine();




            Scribe.PrintColoredAndRegularText(LineInput, "Dene", ConsoleColor.Green);

            //            Console.Write(LineInput);




            // Check if cursorLeftLocate exceeds the buffer size, and adjust it if necessary
            if (cursorLeftLocate + 2 > Console.BufferWidth)
            {
                cursorLeftLocate = Console.BufferWidth - 2;
            }

            //Console.SetCursorPosition(cursorLeftLocate + 2, Console.CursorTop);

            //System.ArgumentOutOfRangeException: 'The value must be greater than or equal to zero and less than the console's buffer size in that dimension. Arg_ParamName_Name
            //ArgumentOutOfRange_ActualValue'
        }
        internal void HandleBackspaceOld(int count = 1) // Old
        {
            for (int i = 0; i < count; i++)
            {
                if (LineInput.Length > 0)
                {
                    LineInput = LineInput.Substring(0, LineInput.Length - 1);
                    Console.Write("\b \b");
                }
                else
                {
                    break;
                }
            }
        }
        internal void HandleBackspace()
        {
            if (LineInput.Length > 0)
            {
                int left = Console.CursorLeft;
                int top = Console.CursorTop;
                int currentLength = LineInput.Length;

                if (left > 2)
                {
                    int endOfInput = left - 3;
                    if (endOfInput >= 0)
                    {
                        LineInput = LineInput.Remove(endOfInput, 1);
                        Console.SetCursorPosition(2, top);
                        Console.Write(LineInput + new string(' ', currentLength - LineInput.Length) + " ");
                        Console.SetCursorPosition(left - 1, top);
                        cursorLeftLocate--;
                    }
                }
            }
        }
    }
}
