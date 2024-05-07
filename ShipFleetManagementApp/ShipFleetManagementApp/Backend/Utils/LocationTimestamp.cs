namespace ShipFleetManagementApp.Backend.Utils
{
    public readonly struct LocationTimestamp(Coordinates position)
    {
        public Coordinates Position { get; } = position;
        public DateTime Time { get; } = DateTime.Now;
    }
}
