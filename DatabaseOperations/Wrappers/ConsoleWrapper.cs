using System;
using DatabaseOperations.Interfaces;

namespace DatabaseOperations.Wrappers
{
    public class ConsoleWrapper : IConsoleWrapper
    {
        public string ReadLine() => Console.ReadLine() ?? string.Empty;
        public void WriteLine(string value) => Console.WriteLine(value);
    }
}
