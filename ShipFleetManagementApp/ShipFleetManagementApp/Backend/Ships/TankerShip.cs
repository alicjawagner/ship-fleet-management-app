using ShipFleetManagementApp.Backend.Ships.Cargo;

namespace ShipFleetManagementApp.Backend.Ships
{
    public class TankerShip : Ship
    {
        public TankerShip(string iMONumber, string name, double length, double width, double latitude, double longitude, double maxLoad, Tank[] tanks)
            : base(iMONumber, name, length, width, latitude, longitude, maxLoad)
        {
            Tanks = tanks;
            foreach (var tank in tanks)
            {
                tank.Ship = this;
            }
        }

        public Tank[] Tanks { get; }

        public override string ToString()
        {
            return "Tanker " + base.ToString();
        }

        /// <summary>
        /// Prints all the tanks installed on the ship.
        /// </summary>
        public void PrintContainers()
        {
            for (int i = 0; i < Tanks.Length; i++)
            {
                Console.WriteLine($"Tank {i}: {Tanks[i]}");
            }
        }
    }
}
