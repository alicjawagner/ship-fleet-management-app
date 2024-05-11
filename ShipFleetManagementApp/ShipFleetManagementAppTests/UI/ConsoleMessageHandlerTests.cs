using ShipFleetManagementApp.Backend.Utils;
using CMH = ShipFleetManagementApp.UI.ConsoleMessageHandler;

namespace ShipFleetManagementAppTests.UI
{
    [TestFixture]
    public class ConsoleMessageHandlerTests
    {
        [Test]
        public void ReadIntInput_ValidInput10_ReturnsInteger10()
        {
            string input = "10";
            StringReader stringReader = new StringReader(input);
            Console.SetIn(stringReader);
            
            int result = CMH.ReadIntInput();

            Assert.That(result, Is.EqualTo(10));
        }

        [TestCase("not_an_integer\n10")]
        [TestCase("4.5\n10")]
        [TestCase("\n10")]
        public void ReadIntInput_InvalidThenValidInput10_ReturnsInteger10(string input)
        {
            StringReader stringReader = new StringReader(input);
            Console.SetIn(stringReader);

            int result = CMH.ReadIntInput();

            Assert.That(result, Is.EqualTo(10));
        }

        [Test]
        public void ReadIntInput_InputForExiting_ThrowsProgramTerminationException()
        {
            string input = "-1";
            StringReader stringReader = new StringReader(input);
            Console.SetIn(stringReader);

            Assert.Throws<ProgramTerminationException>(() => CMH.ReadIntInput());
        }
    }
}
