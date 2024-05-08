namespace ShipFleetManagementApp.Backend.Utils
{
    public readonly struct Coordinates
    {
        public Coordinates(double latitude, double longitude)
        {
            if (latitude >= -90.0 && latitude <= 90.0)
            {
                Latitude = latitude;
            }
            else
            {
                throw new ArgumentException("Latitude is incorrect. It should be in range [-90, 90].");
            }

            if (longitude >= -180.0 && longitude < 180.0)
            {
                Longitude = longitude;
            }
            else
            {
                throw new ArgumentException("Longitude is incorrect. It should be in range [-180, 180).");
            }
        }

        public double Latitude { get; }
        public double Longitude { get; }

        public override string ToString() => $"({Latitude}, {Longitude})";
    }
}
