using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleService.Service.Editor
{
    internal class Read
    {
        Operator keyOp;

        public Read(bool singleKey, bool multiLine = true)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Welcome to Console Service\n");
            Console.Write("Work in progress\n\n");
            Console.ForegroundColor = ConsoleColor.White;
            // CONFIGURATION
            keyOp = new Operator();
            bool newLine = true;
            //
            // LOOP
            do
            {
                Console.CursorVisible = true;
                if (newLine)
                {
                    //if(!history.Any())
                    Console.Write("# ");
                    newLine = false;
                }

                var keyInfo = Console.ReadKey(intercept: true);

                HandleKeyPress(keyInfo, singleKey ? false : multiLine);

                if (keyInfo.Key == ConsoleKey.Enter && !singleKey)
                {
                    if (!multiLine)
                    {
                        break;
                    }
                    
                    //newLine = multiLine ? true : false;
                    newLine = true;
                }

            } while (!singleKey);
        }

        private void HandleKeyPress(ConsoleKeyInfo keyInfo, bool multiLineMode)
        {
            Console.CursorVisible = false;

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                keyOp.HandleEnterKey(multiLineMode);
            }
            else if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                keyOp.HandleUpArrow();
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                keyOp.HandleDownArrow();
            }
            else if (keyInfo.Key == ConsoleKey.Backspace)
            {
                keyOp.HandleBackspace();
            }
            else if (keyInfo.Key == ConsoleKey.Delete)
            {
                keyOp.HandleDeleteKey();
            }
            else if (keyInfo.Key == ConsoleKey.Escape)
            {
                keyOp.ClearCurrentLine();
            }
            else if (keyInfo.Key == ConsoleKey.Tab)
            {
                keyOp.HandleTabKey();
            }
            else if (keyInfo.Key == ConsoleKey.RightArrow)
            {
                keyOp.HandleRightArrowKey();
            }
            else if (keyInfo.Key == ConsoleKey.LeftArrow)
            {
                keyOp.HandleLeftArrowKey();
            }
            else
            {
                keyOp.HandleNormalKey(keyInfo.KeyChar);
            }
        }
    }
}
