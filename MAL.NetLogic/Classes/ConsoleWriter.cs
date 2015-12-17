using System;
using MAL.NetLogic.Interfaces;

namespace MAL.NetLogic.Classes
{
    public class ConsoleWriter : IConsoleWriter
    {
        public void WriteAsLineEnd(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public string WriteInline(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ResetColor();
            return "";
        }
    }
}