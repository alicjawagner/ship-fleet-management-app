namespace ShipFleetManagementApp.Backend.Utils
{
    public struct LocationTimestamp
    {
        public LocationTimestamp(Coordinates position)
        {
            Position = position;
            Time = DateTime.Now;
        }

        public Coordinates Position { get; }
        public DateTime Time { get; }
    }
}
