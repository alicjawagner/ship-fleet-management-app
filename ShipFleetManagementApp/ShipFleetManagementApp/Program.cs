using ShipFleetManagementApp.Backend.Ships;

namespace ShipFleetManagementApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Ship.IsIMONumberCorrect("IMO 9074729"));
        }
    }
}
