namespace ShipFleetManagementApp.Backend.Utils
{
    public class ProgramTerminationException : Exception
    {
        public ProgramTerminationException() : base("User initiated program termination.") { }
    }
}