using System;
using OhceKata.Infrastructure;
using OhceKata.Services;

namespace OhceKata
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new OhceService(new DateTimeProvider(), new MessagePrinter(), () =>
            {
                Console.ReadLine();
                Environment.Exit(0);
            });

            while (true)
            {
                Console.Write("$ ");
                var input = Console.ReadLine();
                service.Ohce(input);
            }
        }
    }
}
