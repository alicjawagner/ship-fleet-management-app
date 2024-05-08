using ShipFleetManagementApp.Backend.Ships.Cargo;

namespace ShipFleetManagementAppTests.Backend.Ships.Cargo
{
    [TestFixture]
    public class FuelTests
    {
        [Test]
        public void Diesel_GotDieselInstance()
        {
            Fuel diesel = Fuel.Diesel;

            Assert.Multiple(() =>
            {
                Assert.That(diesel.Type, Is.EqualTo("Diesel"));
                Assert.That(diesel.Density, Is.EqualTo(0.85));
            });
        }

        [Test]
        public void HeavyFuel_GotHeavyFuellInstance()
        {
            Fuel heavyFuel = Fuel.HeavyFuel;

            Assert.Multiple(() =>
            {
                Assert.That(heavyFuel.Type, Is.EqualTo("Heavy Fuel"));
                Assert.That(heavyFuel.Density, Is.EqualTo(0.95));
            });
        }
    }
}