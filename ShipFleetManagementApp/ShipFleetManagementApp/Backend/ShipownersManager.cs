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
            if (Shipowners.Count == 0) {
                Console.WriteLine("There are no shipowners yet.");
                return;
            }

            Console.WriteLine("Shipowners:");
            for (int i = 0; i < Shipowners.Count; i++)
            {
                Console.WriteLine($"{i}: {Shipowners[i]}");
            }
        }

        /// <summary>
        /// Checks if the shipowner exists.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsShipownerIndexValid(int index)
        {
            return index >= 0 && index < Shipowners.Count;
        }
    }
}
