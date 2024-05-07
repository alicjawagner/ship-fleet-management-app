using ShipFleetManagementApp.Backend.Ships.Cargo;

namespace ShipFleetManagementApp.Backend.Ships
{
    public class TankerShip : Ship
    {
        public Tank[] Tanks { get; }

        public TankerShip(string iMONumber, string name, double length, double width, double latitude, double longitude, double maxLoad, Tank[] tanks)
            : base(iMONumber, name, length, width, latitude, longitude, maxLoad)
        {
            Tanks = tanks;
        }

        public override string ToString()
        {
            return "Tanker " + base.ToString();
        }
    }
}
