namespace ShipFleetManagementApp.Backend.Ships
{
    public class ContainerShip : Ship
    {
        private int _maxContainers;
        private int _currentContainers = 0;

        public ContainerShip()
            : base()
        {
        }

        public ContainerShip(string iMONumber, string name, double length, double width, double latitude, double longitude, double maxLoad, int maxContainers)
            : base(iMONumber, name, length, width, latitude, longitude, maxLoad)
        {
            MaxContainers = maxContainers;
        }

        public int MaxContainers
        {
            get { return _maxContainers; }
            set
            {
                if (value >= 0)
                {
                    _maxContainers = value;
                }
                else
                {
                    throw new ArgumentException("MaxContainers is incorrect. It should be a non-negative number.");
                }
            }
        }

        public int CurrentContainers
        {
            get { return _currentContainers; }
            set
            {
                if (value >= 0 && value <= MaxContainers)
                {
                    _currentContainers = value;
                }
                else
                {
                    throw new ArgumentException("CurrentContainers is incorrect. It should be less or equal to MaxContainers and a non-negative number.");
                }
            }
        }
    }
}
