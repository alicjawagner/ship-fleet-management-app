using ShipFleetManagementApp.Backend;
using ShipFleetManagementApp.Backend.Ships;

namespace ShipFleetManagementApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Ship.IsIMONumberCorrect("IMO 9074729"));
            Ship ship = new ContainerShip("IMO 9074729", "Black Pearl", 366, 49, 40.7128, -74.0060, 300000, 20000);
            ContainerShip ship2 = new ContainerShip("IMO 9074729", "Black Pearl", 366, 49, 40.7128, -74.0060, 300000, 20000);
            Shipowner s = new Shipowner("f");
            s.AddShip(ship);
            s.AddShip(ship2);

        }
    }
}
