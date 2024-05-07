namespace ShipFleetManagementApp.Backend.Ships.Cargo
{
    public readonly struct Container
    {
        public Container(string sender, string adressee, string cargoDescription, double weight)
        {
            Sender = sender;
            Adressee = adressee;
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
        public string Adressee { get; }
        public string CargoDescription { get; }
        public double Weight { get; }
    }
}
