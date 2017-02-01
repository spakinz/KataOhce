using System;

namespace OhceKata.Infrastructure
{
    public class MessagePrinter : IMessagePrinter
    {
        public void Print(string message)
        {
            Console.WriteLine($"> {message}");
        }
    }
}