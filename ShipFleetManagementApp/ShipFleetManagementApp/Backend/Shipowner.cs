using ShipFleetManagementApp.Backend.Ships;

namespace ShipFleetManagementApp.Backend
{
    public class Shipowner
    {
        public Shipowner(string name)
        {
            Name = name;
        }

        public string Name {  get; }
        public List<Ship> Ships { get; } = [];

        /// <summary>
        /// Adds a ship of any type to the shipowner's ship list.
        /// </summary>
        /// <param name="ship"></param>
        private void AddShip(Ship ship)
        {
            Ships.Add(ship);
        }

        /// <summary>
        /// Adds a container ship to the shipowner's ship list.
        /// </summary>
        /// <param name="iMONumber"></param>
        /// <param name="name"></param>
        /// <param name="length"></param>
        /// <param name="width"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="maxLoad"></param>
        /// <param name="maxContainers"></param>
        public void AddContainerShip(string iMONumber, string name, double length, double width, double latitude, double longitude, double maxLoad, int maxContainers)
        {
            ContainerShip ship = new ContainerShip(iMONumber, name, length, width, latitude, longitude, maxLoad, maxContainers);
            AddShip(ship);
        }

        /// <summary>
        /// Adds a tanker ship to the shipowner's ship list.
        /// </summary>
        /// <param name="iMONumber"></param>
        /// <param name="name"></param>
        /// <param name="length"></param>
        /// <param name="width"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="maxLoad"></param>
        /// <param name="tanks"></param>
        public TankerShip AddTankerShip(string iMONumber, string name, double length, double width, double latitude, double longitude, double maxLoad)
        {
            TankerShip ship = new TankerShip(iMONumber, name, length, width, latitude, longitude, maxLoad);
            AddShip(ship);
            return ship;
        }

        /// <summary>
        /// Prints all ships of the shipowner.
        /// </summary>
        public void PrintShips()
        {
            if (Ships.Count == 0)
            {
                Console.WriteLine($"There are no ships belonging to {Name} yet.");
                return;
            }

            Console.WriteLine($"{Name}'s ships:");
            for (int i = 0; i < Ships.Count; i++)
            {
                Console.WriteLine($"{i}: {Ships[i]}");
            }
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
