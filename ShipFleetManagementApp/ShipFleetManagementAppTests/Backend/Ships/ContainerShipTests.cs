using ShipFleetManagementApp.Backend.Ships;
using ShipFleetManagementApp.Backend.Utils;

namespace ShipFleetManagementAppTests.Backend.Ships
{
    [TestFixture]
    public class ContainerShipTests
    {
        private ContainerShip _ship;

        [SetUp]
        public void Init()
        {
            _ship = new ContainerShip("IMO 9074729", "Black Pearl", 366, 49, 40.7128, -74.0060, 300000, 20000);
        }

        [Test]
        public void Constructor_ValidValues_CreatesObject()
        {
            string iMONumber = "IMO 1234567";
            string name = "Black Pearl";
            double length = 366;
            double width = 49;
            double latitude = 40.7128;
            double longitude = -74.0060;
            double maxLoad = 300000;
            int maxContainers = 20000;

            var containerShip = new ContainerShip(iMONumber, name, length, width, latitude, longitude, maxLoad, maxContainers);

            Assert.Multiple(() =>
            {
                Assert.That(containerShip.IMONumber, Is.EqualTo(iMONumber));
                Assert.That(containerShip.Name, Is.EqualTo(name));
                Assert.That(containerShip.Length, Is.EqualTo(length));
                Assert.That(containerShip.Width, Is.EqualTo(width));
                Assert.That(containerShip.MaxLoad, Is.EqualTo(maxLoad));
                Assert.That(containerShip.MaxContainers, Is.EqualTo(maxContainers));
                Assert.That(containerShip.CurrentContainers, Is.EqualTo(0));
                Assert.That(containerShip.CurrentPosition, Is.EqualTo(new Coordinates(latitude, longitude)));
            });
        }

        [TestCase("IMO 90747299", "Black Pearl", 366, 49, 40.7128, -74.0060, 300000, 20000)] // invalid IMO Number
        [TestCase("IMO 9074729", "Black Pearl", -1, 49, 40.7128, -74.0060, 300000, 20000)] // invalid length
        [TestCase("IMO 9074729", "Black Pearl", 366, -1, 40.7128, -74.0060, 300000, 20000)] // invalid width
        [TestCase("IMO 9074729", "Black Pearl", 366, 49, 91.0, -74.0060, 300000, 20000)] // invalid latitude
        [TestCase("IMO 9074729", "Black Pearl", 366, 49, 40.7128, 180.0, 300000, 20000)] // invalid longitude
        [TestCase("IMO 9074729", "Black Pearl", 366, 49, 40.7128, -74.0060, -1, 20000)] // invalid maxLoad
        [TestCase("IMO 9074729", "Black Pearl", 366, 49, 40.7128, -74.0060, 300000, -1)] // invalid maxContainers
        public void Constructor_InvalidValues_ThrowsException(string iMONumber, string name, double length, double width, double latitude, double longitude, double maxLoad, int maxContainers)
        {
            Assert.Throws<ArgumentException>(() => new ContainerShip(iMONumber, name, length, width, latitude, longitude, maxLoad, maxContainers));
        }

        [Test]
        public void Constructor_NotUniqueIMONumber_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new ContainerShip("IMO 9074729", "Black Pearl2", 367, 56, 40.7128, -74.0060, 300000, 20000));
        }

        [Test]
        public void LoadContainer_ValidContainer_AddsContainerAndUpdatesCurrentContainersAndCurrentLoad()
        {
            double initialLoad = _ship.CurrentLoad;

            _ship.LoadContainer("Sender", "Addressee", "Cargo", 100);

            Assert.Multiple(() =>
            {
                Assert.That(_ship.CurrentLoad, Is.EqualTo(initialLoad + 100));
                Assert.That(_ship.CurrentContainers, Is.EqualTo(1));
            });
        }

        [Test]
        public void LoadContainer_InvalidWegihtOfContainer_ThrowsExceptionAndDoesNotAddContainer()
        {
            double initialLoad = _ship.CurrentLoad;

            // negative weight
            Assert.Throws<ArgumentException>(() => _ship.LoadContainer("Sender", "Addressee", "Cargo", -1));
            Assert.Multiple(() =>
            {
                Assert.That(_ship.CurrentLoad, Is.EqualTo(initialLoad));
                Assert.That(_ship.CurrentContainers, Is.EqualTo(0));
                Assert.That(_ship.Containers, Is.Empty);
            });
        }

        [Test]
        public void LoadContainer_ExceededPermittedLoad_ThrowsExceptionAndDoesNotAddContainer()
        {
            double initialLoad = _ship.CurrentLoad;

            Assert.Throws<InvalidOperationException>(() => _ship.LoadContainer("Sender", "Addressee", "Cargo", _ship.MaxLoad + 1));
            Assert.Multiple(() =>
            {
                Assert.That(_ship.CurrentLoad, Is.EqualTo(initialLoad));
                Assert.That(_ship.CurrentContainers, Is.EqualTo(0));
                Assert.That(_ship.Containers, Is.Empty);
            });
        }

        [Test]
        public void LoadContainer_ExceededPermittedNumberOfContainers_ThrowsExceptionAndDoesNotAddContainer()
        {
            _ship = new ContainerShip("IMO 1234567", "Black Pearl", 366, 49, 40.7128, -74.0060, 300000, 0);
            double initialLoad = _ship.CurrentLoad;

            Assert.Throws<InvalidOperationException>(() => _ship.LoadContainer("Sender", "Addressee", "Cargo", 100));
            Assert.Multiple(() =>
            {
                Assert.That(_ship.CurrentLoad, Is.EqualTo(initialLoad));
                Assert.That(_ship.CurrentContainers, Is.EqualTo(0));
                Assert.That(_ship.Containers, Is.Empty);
            });
        }

        [Test]
        public void UnloadContainer_ValidIndex_RemovesContainerAndUpdatesCurrentContainersAndCurrentLoad()
        {
            _ship.LoadContainer("Sender", "Addressee", "Cargo", 100);
            double initialLoad = _ship.CurrentLoad;
            int initialContainersCount = _ship.CurrentContainers;

            _ship.UnloadContainer(0);

            Assert.Multiple(() =>
            {
                Assert.That(_ship.CurrentLoad, Is.EqualTo(initialLoad - 100));
                Assert.That(_ship.CurrentContainers, Is.EqualTo(initialContainersCount - 1));
                Assert.That(_ship.Containers, Is.Empty);
            });
        }
        
        [Test]
        public void UnloadContainer_InvalidIndex_ThrowsExceptionAndDoesNotModifyCurrentContainersAndCurrentLoad()
        {
            _ship.LoadContainer("Sender", "Addressee", "Cargo", 100);
            double initialLoad = _ship.CurrentLoad;
            int initialContainersCount = _ship.CurrentContainers;

            Assert.Throws<InvalidOperationException>(() => _ship.UnloadContainer(1));
            Assert.Multiple(() =>
            {
                Assert.That(_ship.CurrentLoad, Is.EqualTo(initialLoad));
                Assert.That(_ship.CurrentContainers, Is.EqualTo(initialContainersCount));
                Assert.That(_ship.Containers, Has.Count.EqualTo(1));
            });
        }

        [TearDown]
        public void TearDown()
        {
            Ship.IMONumbersUsed.Clear();
        }
    }
}
