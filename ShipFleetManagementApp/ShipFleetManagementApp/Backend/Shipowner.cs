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
        public void AddShip(Ship ship)
        {
            Ships.Add(ship);
        }

        /// <summary>
        /// Prints all ships of the shipowner.
        /// </summary>
        public void PrintShips()
        {
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
