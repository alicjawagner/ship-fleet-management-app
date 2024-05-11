using ShipFleetManagementApp.Backend.Ships;
using ShipFleetManagementApp.Backend.Ships.Cargo;

namespace ShipFleetManagementAppTests.Backend.Ships.Cargo
{
    [TestFixture]
    public class TankTests
    {
        private TankerShip _ship;
        /// <summary>
        /// With max capacity 100000, installed on _ship.
        /// </summary>
        private Tank _tank;

        [SetUp]
        public void SetUp()
        {
            _tank = new Tank(100000);
            List<Tank> tanksList = [_tank];
            _ship = new TankerShip("IMO 9074729", "Black Pearl", 366, 49, 40.7128, -74.0060, 80);
            _ship.Tanks = tanksList;
        }

        [Test]
        public void RefuelTank_ValidValues_UpdatesTankPropertiesAndUpdatesCurrentLoad()
        {
            double initialLoad = _ship.CurrentLoad;

            _tank.RefuelTank(Fuel.Diesel, 100);

            Assert.Multiple(() =>
            {
                Assert.That(_tank.FuelType, Is.EqualTo(Fuel.Diesel));
                Assert.That(_tank.CurrentVolume, Is.EqualTo(100));
                Assert.That(_ship.CurrentLoad, Is.EqualTo(initialLoad + 0.085)); // + weight of the fuel
            });
        }
        
        [Test]
        public void RefuelTank_ExceededPermittedLoad_ThrowsExceptionAndDoesNotModifyTank()
        {
            double initialLoad = _ship.CurrentLoad;

            Assert.Throws<InvalidOperationException>(() => _tank.RefuelTank(Fuel.Diesel, 100000));
            Assert.Multiple(() =>
            {
                Assert.That(_ship.CurrentLoad, Is.EqualTo(initialLoad));
                Assert.That(_tank.FuelType, Is.Null);
                Assert.That(_tank.CurrentVolume, Is.EqualTo(0));
            });
        }
    }
}