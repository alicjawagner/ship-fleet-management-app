namespace ShipFleetManagementApp.Backend
{
    public class ShipownersManager
    {
        public ShipownersManager()
        {
        }

        public List<Shipowner> Shipowners { get; } = [];
        
        /// <summary>
        /// Registers the new shipowner.
        /// </summary>
        /// <param name="ship"></param>
        public void AddShipowner(Shipowner shipowner)
        {
            Shipowners.Add(shipowner);
        }

        /// <summary>
        /// Prints all shipowners.
        /// </summary>
        public void PrintShipowners()
        {
            for (int i = 0; i < Shipowners.Count; i++)
            {
                Console.WriteLine($"{i}: {Shipowners[i]}");
            }
        }
    }
}
