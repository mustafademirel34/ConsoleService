using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleService.Service.Editor
{
    internal class Scribe
    {
        internal static void PrintColoredLine(string text, ConsoleColor color = ConsoleColor.White, bool newLine = false)
        {
            ColorOfConsole(color);
            if (newLine)
                TextToConsole(text);
            else
                TextToConsole(text);
            ColorOfConsole(ConsoleColor.White);
        }

        internal static void PrintColoredAndRegularText(string text, string coloredText, ConsoleColor color)
        {
            int startIndex = text.IndexOf(coloredText);
            if (startIndex >= 0)
            {
                TextToConsole(text.Substring(0, startIndex));
                ColorOfConsole(color);
                TextToConsole(coloredText);
                ColorOfConsole(ConsoleColor.White);
                TextToConsole(text.Substring(startIndex + coloredText.Length));
            }
            else
                TextToConsole(text);
        }

        internal static void PrintMultiColoredText(string fullText, Dictionary<string, ConsoleColor> colorMappings, bool newLine = true)
        {
            int currentPosition = 0;
            foreach (var mapping in colorMappings)
            {
                int nextPosition = fullText.IndexOf(mapping.Key, currentPosition);
                if (nextPosition >= 0)
                {
                    TextToConsole(fullText.Substring(currentPosition, nextPosition - currentPosition));
                    ColorOfConsole(mapping.Value);
                    TextToConsole(mapping.Key);
                    ColorOfConsole(ConsoleColor.White);
                    currentPosition = nextPosition + mapping.Key.Length;
                }
            }
            if (currentPosition < fullText.Length)
            {
                TextToConsole(fullText.Substring(currentPosition));
            }
            if (newLine)
                TextToConsole("", true);
        }

        internal static void ErrorText(string text)
        {
            ColorOfConsole(ConsoleColor.Red);
            TextToConsole(text);
            ResetColorOfConsole();
        }

        internal static void OutputText(string text)
        {
            ColorOfConsole(ConsoleColor.Cyan);
            TextToConsole(text);
            ResetColorOfConsole();
        }

        internal static void SuccessText(string text)
        {
            ColorOfConsole(ConsoleColor.Green);
            TextToConsole(text);
            ResetColorOfConsole();
        }

        private static void TextToConsole(string text = "", bool newLine = false)
        {
            if (newLine)
                Console.WriteLine(text);
            else
                Console.Write(text);
        }

        private static void ColorOfConsole(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        private static void ResetColorOfConsole()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}

