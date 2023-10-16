using ConsoleTesting.Test;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTesting.Core
{
    public class Utilities 
    {

        public static string[] RemoveValueFromArray(string[] array, string value)
        {
            return array.Where(item => item != value).ToArray();
        }

        public static bool ContainsAny(string key, params string[] searchTerms)
        {
            foreach (var term in searchTerms)
            {
                if (key.Contains(term))
                    return true;
            }
            return false;
        }

        public static string GetRemainingString(string fullString, string delimiter)
        {
            int index = fullString.IndexOf(delimiter);
            if (index != -1)
            {
                return fullString.Substring(index + delimiter.Length).Trim();
            }
            return fullString;
        }

        public static void PrintColoredLine(string text, ConsoleColor color, bool newLine = false, bool result = true)
        {
            if (result)
                text = " > " + text;
            Console.ForegroundColor = color;
            if (newLine)
                Console.WriteLine(text);
            else
                Console.Write(text);
            Console.ForegroundColor = ConsoleColor.White;
        }
        internal static void WelcomeScreen()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Welcome to Console Testing");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Use the 'help' method for information on how to use it");
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void PrintHelpInstructions()
        {
            Console.WriteLine("Will be edited");

            //var colorMappings = new Dictionary<string, ConsoleColor>
            //{
            //    { "/0", ConsoleColor.Cyan },
            //    { "-", ConsoleColor.Magenta },
            //    { "dinf", ConsoleColor.Blue },
            //    { "cinf", ConsoleColor.Blue },
            //    { "space", ConsoleColor.Cyan },
            //    { "clear", ConsoleColor.Red },
            //};

        }

        public static void PrintColoredAndRegularText(string fullText, string coloredText, ConsoleColor color)
        {
            int startIndex = fullText.IndexOf(coloredText);
            if (startIndex >= 0)
            {
                Console.Write(fullText.Substring(0, startIndex));
                PrintColoredLine(coloredText, color);
                Console.Write(fullText.Substring(startIndex + coloredText.Length));
            }
            else
                Console.Write(fullText);
        }

        public static void PrintMultiColoredText(string text, Dictionary<string, ConsoleColor> colorMappings, bool newLine = true)
        {
            int currentPosition = 0;
            foreach (var mapping in colorMappings)
            {
                int nextPosition = text.IndexOf(mapping.Key, currentPosition);

                if (nextPosition >= 0)
                {
                    Console.Write(text.Substring(currentPosition, nextPosition - currentPosition));
                    PrintColoredLine(mapping.Key, mapping.Value);
                    currentPosition = nextPosition + mapping.Key.Length;
                }
            }
            if (currentPosition < text.Length)
            {
                Console.Write(text.Substring(currentPosition));
            }
            if (newLine)
                Console.WriteLine();
        }


    }
}
