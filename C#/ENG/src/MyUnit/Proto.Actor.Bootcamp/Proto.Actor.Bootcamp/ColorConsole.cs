using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proto.Actor.Bootcamp
{
    public static class ColorConsole
    {
        public static void WriteLineGreen(string message)
        {
            var beforeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = beforeColor;
        }

        public static void WriteLineYellow(string message)
        {
            var beforeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ForegroundColor = beforeColor;
        }

        public static void WriteLineRed(string message)
        {
            var beforeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = beforeColor;
        }
        public static void WriteLineGray(string message)
        {
            var beforeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(message);
            Console.ForegroundColor = beforeColor;
        }

        public static void WriteLineCyan(string message)
        {
            var beforeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(message);
            Console.ForegroundColor = beforeColor;
        }

        internal static void WriteMagenta(string message)
        {
            var beforeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(message);
            Console.ForegroundColor = beforeColor;
        }
    }
}
