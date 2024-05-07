using ShipFleetManagementApp.Backend.Ships;

namespace ShipFleetManagementAppTests.Backend.Ships
{
    [TestFixture]
    public class ShipTests
    {
        [TestCase("INVALID")]
        [TestCase("IMO 123456")] // less than 7 digits
        [TestCase("IMO_9074729")] // no space
        [TestCase("IMO  9074729")] // 2 spaces
        [TestCase("IMO 90747299")] // 8 digits
        [TestCase("iMO 123456")] // lowercase letter
        [TestCase("IMO 907e729")] // letter between the numbers
        public void IsIMONumberCorrect_IncorrectValues_ReturnFalse(string iMONumber)
        {
            var result = Ship.IsIMONumberCorrect(iMONumber);

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsIMONumberCorrect_CorrectValue_ReturnTrue()
        {
            var result = Ship.IsIMONumberCorrect("IMO 9074729");

            Assert.That(result, Is.True);
        }

        
    }
}
