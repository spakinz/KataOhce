using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using OhceKata.Infrastructure;
using OhceKata.Services;

namespace OhceKata.Tests
{
    [TestFixture]
    public class OhceServiceTest
    {
        private Mock<IMessagePrinter> _consoleProviderMock;
        private List<string> _consoleMessages;

        [SetUp]
        public void TestInitialize()
        {
            _consoleProviderMock = new Mock<IMessagePrinter>();
            _consoleMessages = new List<string>();
            _consoleProviderMock.Setup(cp => cp.Print(It.IsAny<string>())).Callback((string x) => _consoleMessages.Add(x));
        }

        [Test]
        public void OhceThrowsOnNullMessage()
        {
            var service = new OhceService(null, _consoleProviderMock.Object, null);
            Assert.Throws<ArgumentNullException>(() => service.Ohce(null));
        }

        [Test]
        public void OhceReturnsAlohWhenPassingHola()
        {
            var service = new OhceService(null, _consoleProviderMock.Object, null);
            service.Ohce("hola");
            _consoleMessages.Should().HaveCount(1);
            _consoleMessages.First().Should().Be("aloh");
        }

        [Test]
        public void OhceReturnsBonitaPalabraWhenPalindromo()
        {
            var service = new OhceService(null, _consoleProviderMock.Object, null);
            service.Ohce("oto");
            _consoleMessages.Should().HaveCount(2);
            _consoleMessages.First().Should().Be("oto");
            _consoleMessages.Last().Should().Be("¡Bonita palabra!");
        }

        [Test]
        public void OhceReturnsBuenosDias()
        {
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(dt => dt.GetDateTime()).Returns(new DateTime(2017, 1, 31, 9, 0, 0));
            var service = new OhceService(dateTimeProviderMock.Object, _consoleProviderMock.Object, null);
            service.Ohce("ohce Pedro");
            _consoleMessages.Should().HaveCount(1);
            _consoleMessages.Single().Should().Be("¡Buenos días Pedro!");
        }

        [Test]
        public void OhceReturnsBuenasTardes()
        {
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(dt => dt.GetDateTime()).Returns(new DateTime(2017, 1, 31, 19, 0, 0));
            var service = new OhceService(dateTimeProviderMock.Object, _consoleProviderMock.Object, null);
            service.Ohce("ohce Pedro");
            _consoleMessages.Should().HaveCount(1);
            _consoleMessages.Single().Should().Be("¡Buenas tardes Pedro!");
        }

        [Test]
        public void OhceReturnsBuenasNoches()
        {
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(dt => dt.GetDateTime()).Returns(new DateTime(2017, 1, 31, 23, 0, 0));
            var service = new OhceService(dateTimeProviderMock.Object, _consoleProviderMock.Object, null);
            service.Ohce("ohce Pedro");
            _consoleMessages.Should().HaveCount(1);
            _consoleMessages.Single().Should().Be("¡Buenas noches Pedro!");
        }

        [Test]
        public void OhceReturnsGoodbyeMessage()
        {
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(dt => dt.GetDateTime()).Returns(new DateTime(2017, 1, 31, 23, 0, 0));
            var service = new OhceService(dateTimeProviderMock.Object, _consoleProviderMock.Object, () => {});
            service.Ohce("ohce Pedro");
            _consoleMessages.Clear();
            service.Ohce("Stop!");
            _consoleMessages.Should().HaveCount(1);
            _consoleMessages.Single().Should().Be("Adios Pedro");
        }

        [Test]
        public void OhceCallsStopActionOnGoodbye()
        {
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(dt => dt.GetDateTime()).Returns(new DateTime(2017, 1, 31, 23, 0, 0));
            var hasCalledStopAction = false;
            var service = new OhceService(dateTimeProviderMock.Object, _consoleProviderMock.Object, () =>
            {
                hasCalledStopAction = true;
            });
            service.Ohce("ohce Pedro");
            _consoleMessages.Clear();
            service.Ohce("Stop!");
            hasCalledStopAction.Should().BeTrue();
        }
    }
}
