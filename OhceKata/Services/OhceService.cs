using System;
using System.Linq;
using OhceKata.Infrastructure;

namespace OhceKata.Services
{
    public class OhceService
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMessagePrinter _messagePrinter;
        private readonly Action _onStopAction;
        private string _userName;

        public OhceService(IDateTimeProvider dateTimeProvider, IMessagePrinter messagePrinter, Action onStopAction)
        {
            _dateTimeProvider = dateTimeProvider;
            _messagePrinter = messagePrinter;
            _onStopAction = onStopAction;
            _userName = "Unknown User";
        }

        public void Ohce(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (IsAGreetingMessage(input))
            {
                HandleGreetingMessage(input);
            }
            else if (IsStopMessage(input))
            {
                HandleStopMessage();
                _onStopAction?.Invoke();
            }
            else
            {
                var result = string.Join(string.Empty, input.Reverse());

                _messagePrinter.Print(result);

                if (IsPalindrome(input, result))
                {
                    HandlePalindrome();
                }
            }
        }

        private void HandlePalindrome()
        {
            _messagePrinter.Print("¡Bonita palabra!");
        }

        private bool IsPalindrome(string input, string result)
        {
            return string.Equals(input, result);
        }

        private void HandleStopMessage()
        {
            _messagePrinter.Print($"Adios {_userName}");
        }

        private bool IsStopMessage(string input)
        {
            return string.Equals(input, "Stop!");
        }

        private void HandleGreetingMessage(string input)
        {
            var strings = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            _userName = strings[1];

            var specificMessage = GetSpecificMessage();

            _messagePrinter.Print($"¡{specificMessage} {_userName}!");
        }

        private bool IsAGreetingMessage(string input)
        {
            return input.StartsWith("ohce");
        }

        private string GetSpecificMessage()
        {
            var hour = _dateTimeProvider.GetDateTime().Hour;
            var specificMessage = "Buenas noches";

            if (hour >= 6 && hour < 12)
            {
                specificMessage = "Buenos días";
            }
            else if (hour >= 12 && hour < 20)
            {
                specificMessage = "Buenas tardes";
            }
            return specificMessage;
        }
    }
}
