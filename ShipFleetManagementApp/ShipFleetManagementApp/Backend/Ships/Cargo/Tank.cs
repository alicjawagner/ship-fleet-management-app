namespace ShipFleetManagementApp.Backend.Ships.Cargo
{
    public class Tank
    {
        /// <summary>
        /// Current weight of the tank in tons.
        /// </summary>
        private double _currentWeight;
        private double _currentVolume;
        private double _maxCapacity;

        public Tank(double maxCapacity)
        {
            MaxCapacity = maxCapacity;
            _currentVolume = 0;
            _currentWeight = 0;
        }

        /// <summary>
        /// Ship which the tank is loaded onto.
        /// </summary>
        public TankerShip? Ship { get; set; }

        /// <summary>
        /// Type of fuel currently refueled.
        /// </summary>
        public Fuel? FuelType { get; set; }

        /// <summary>
        /// Maximum permitted capacity in liters.
        /// </summary>
        public double MaxCapacity
        {
            get { return _maxCapacity; }
            private set
            {
                if (value >= 0)
                {
                    _maxCapacity = value;
                }
                else
                {
                    throw new ArgumentException("MaxCapacity is incorrect. It should be a non-negative number.");
                }
            }
        }

        /// <summary>
        /// Current volume covered by the fuel in liters.
        /// </summary>
        public double CurrentVolume
        {
            get { return _currentVolume; }
            set
            {
                if (value >= 0 && value <= MaxCapacity)
                {
                    _currentVolume = value;
                }
                else
                {
                    throw new ArgumentException("CurrentVolume is incorrect. It should be less or equal to MaxLoad and a non-negative number.");
                }
            }
        }

        /// <summary>
        /// Fills the tank with the specified number of liters of specified fuel.
        /// </summary>
        /// <param name="fuelType"></param>
        /// <param name="liters"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void RefuelTank(Fuel fuelType, double liters)
        {
            if (Ship == null)
            {
                throw new InvalidOperationException("Refueling tank unsuccessful. The tank is not installed on any ship.");
            }
            if (CurrentVolume != 0)
            {
                throw new InvalidOperationException("Refueling tank unsuccessful. The tank has already been refueled.");
            }

            FuelType = fuelType;
            CurrentVolume = liters;
            double tankWeight = CalculateWeight();
            try
            {
                Ship.CurrentLoad += tankWeight;
                _currentWeight = tankWeight;
            }
            catch (ArgumentException)
            {
                FuelType = null;
                CurrentVolume = 0;
                throw new InvalidOperationException("Refueling tank unsuccessful. The total weight exceeds the permitted load.");
            }
        }

        /// <summary>
        /// Completely empties the tank.
        /// </summary>
        public void EmptyTank()
        {
            if (Ship == null)
            {
                throw new InvalidOperationException("Emptying tank unsuccessful. The tank is not installed on any ship.");
            }

            Ship.CurrentLoad -= _currentWeight;
            FuelType = null;
            CurrentVolume = 0;
            _currentWeight = 0;
        }

        public override string ToString()
        {
            string state = (CurrentVolume != 0) ? $"refueled ({FuelType!.Type} - {CurrentVolume} liters)" : "empty";
            return $"Max capacity: {MaxCapacity} liters, state: {state}";
        }

        /// <summary>
        /// Calculates the weight of the currently refueled fuel in tons.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private double CalculateWeight()
        {
            if (FuelType == null)
            {
                throw new InvalidOperationException("Calculating weight for a tank unsuccessful. First, choose the type of fuel.");
            }
            return (CurrentVolume * FuelType.Density) / 1000;
        }
    }
}
