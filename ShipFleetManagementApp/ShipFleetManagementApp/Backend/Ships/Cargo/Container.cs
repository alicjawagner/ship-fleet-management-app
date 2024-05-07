namespace ShipFleetManagementApp.Backend.Ships.Cargo
{
    public readonly struct Container
    {
        public Container(string sender, string addressee, string cargoDescription, double weight)
        {
            Sender = sender;
            Addressee = addressee;
            CargoDescription = cargoDescription;
            if (weight >= 0)
            {
                Weight = weight;
            }
            else
            {
                throw new ArgumentException("Weight is incorrect. It should be a non-negative number.");
            }
        }

        public string Sender { get; }
        public string Addressee { get; }
        public string CargoDescription { get; }
        public double Weight { get; }

        public override string ToString()
        {
            return $"Container - Sender: {Sender}, Addressee: {Addressee}, Cargo Description: {CargoDescription}, Weight: {Weight} tons";
        }
    }
}
