using System;
using DatabaseOperations.Core.Interfaces;

namespace DatabaseOperations.Core.Wrappers
{
    public class ConsoleWrapper : IConsoleWrapper
    {
        public string ReadLine() => Console.ReadLine() ?? string.Empty;
        public void WriteLine(string value) => Console.WriteLine(value);
    }
}
