﻿using ShipFleetManagementApp.Backend.Ships.Cargo;

namespace ShipFleetManagementApp.Backend.Ships
{
    public class TankerShip : Ship
    {
        private Tank[]? _tanks = null;

        public TankerShip(string iMONumber, string name, double length, double width, double latitude, double longitude, double maxLoad)
            : base(iMONumber, name, length, width, latitude, longitude, maxLoad)
        {
        }

        /// <summary>
        /// The list of the tanks installed on the ship.
        /// </summary>
        public Tank[] Tanks {
            get
            {
                return _tanks ?? [];
            }
            set
            {
                if (_tanks == null)
                {
                    _tanks = value;
                    foreach (Tank tank in value)
                    {
                        tank.Ship = this;
                    }
                }
            }
        }

        /// <summary>
        /// Prints all the tanks installed on the ship.
        /// </summary>
        public void PrintTanks()
        {
            if (Tanks.Length == 0)
            {
                Console.WriteLine("There are no tanks installed on this ship.");
                return;
            }

            for (int i = 0; i < Tanks.Length; i++)
            {
                Console.WriteLine($"Tank {i}: {Tanks[i]}");
            }
        }

        public override string ToString()
        {
            return "Tanker " + base.ToString();
        }
    }
}
