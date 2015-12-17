using System;

namespace MAL.NetLogic.Interfaces
{
    public interface IConsoleWriter
    {
        void WriteAsLineEnd(string message, ConsoleColor color);
        string WriteInline(string message, ConsoleColor color);
    }
}