using ShipFleetManagementApp.Backend.Utils;

namespace ShipFleetManagementApp.Backend.Ships
{
    public abstract class Ship
    {
        private string? _iMONumber;
        private double _length;
        private double _width;
        private double _maxLoad;
        private double _currentLoad;

        protected Ship(string iMONumber, string name, double length, double width, double latitude, double longitude, double maxLoad)
        {
            PositionHistory = [];
            _currentLoad = 0;
            IMONumber = iMONumber;
            Name = name;
            Length = length;
            Width = width;
            UpdatePosition(latitude, longitude);
            MaxLoad = maxLoad;
        }

        /// <summary>
        /// Name of the ship.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Current position of the ship (latitude, longitude).
        /// </summary>
        public Coordinates CurrentPosition { get; private set; }

        /// <summary>
        /// History of the ship's position with update time.
        /// </summary>
        public List<LocationTimestamp> PositionHistory { get; }

        /// <summary>
        /// IMO Number of the ship.
        /// </summary>
        public string IMONumber
        {
            get
            {
                return _iMONumber ?? "unspecified";
            }
            private set
            {
                if (IsIMONumberCorrect(value))
                {
                    _iMONumber = value.Trim();
                }
                else
                {
                    throw new ArgumentException($"IMO number {value} is incorrect. It should start with capital letters 'IMO', followed by a space character, followed by 7 valid digits.");
                }
            }
        }

        /// <summary>
        /// Length in meters.
        /// </summary>
        public double Length
        {
            get { return _length; }
            private set
            {
                if (value > 0)
                {
                    _length = value;
                }
                else
                {
                    throw new ArgumentException($"Length {value} is incorrect. It should be a positive number.");
                }
            }
        }

        /// <summary>
        /// Width in meters.
        /// </summary>
        public double Width
        {
            get { return _width; }
            private set
            {
                if (value > 0)
                {
                    _width = value;
                }
                else
                {
                    throw new ArgumentException($"Width {value} is incorrect. It should be a positive number.");
                }
            }
        }

        /// <summary>
        /// Maximum permitted load in tons.
        /// </summary>
        public double MaxLoad
        {
            get { return _maxLoad; }
            private set
            {
                if (value >= 0)
                {
                    _maxLoad = value;
                }
                else
                {
                    throw new ArgumentException($"MaxLoad {value} is incorrect. It should be a non-negative number.");
                }
            }
        }

        /// <summary>
        /// Current load in tons.
        /// </summary>
        public double CurrentLoad
        {
            get { return _currentLoad; }
            set
            {
                if (value >= 0 && value <= MaxLoad)
                {
                    _currentLoad = value;
                }
                else
                {
                    throw new ArgumentException($"CurrentLoad {value} is incorrect. It should be less or equal to MaxLoad and a non-negative number.");
                }
            }
        }

        /// <summary>
        /// Checks if the IMO Number is correct (check digit, format).
        /// </summary>
        /// <param name="iMONumber"></param>
        /// <returns></returns>
        public static bool IsIMONumberCorrect(string iMONumber)
        {
            if (string.IsNullOrEmpty(iMONumber))
            {
                return false;
            }

            iMONumber = iMONumber.Trim();
            if (!iMONumber.StartsWith("IMO "))
            {
                return false;
            }

            string numbersPart = iMONumber[4..];
            if (numbersPart.Length != 7)
            {
                return false;
            }

            int sum = 0;

            for (int i = 0, factor = 7; i < numbersPart.Length - 1; i++, factor--)
            {
                char c = numbersPart[i];
                if (char.IsDigit(c))
                {
                    int digit = c - '0';
                    sum += digit * factor;
                }
                else
                {
                    return false;
                }
            }

            char lastChar = numbersPart[^1];
            int lastDigit = lastChar - '0';
            return sum % 10 == lastDigit;
        }

        /// <summary>
        /// Updates the ship's position and adds an entry to the position history.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        public void UpdatePosition(double latitude, double longitude)
        {
            Coordinates position = new Coordinates(latitude, longitude);
            CurrentPosition = position;
            PositionHistory.Add(new LocationTimestamp(position));
        }

        /// <summary>
        /// Prints the whole position history.
        /// </summary>
        public void PrintPositionHistory()
        {
            Console.WriteLine($"The position history of {IMONumber} {Name}:");
            foreach (LocationTimestamp timestamp in PositionHistory)
            {
                Console.WriteLine(timestamp.ToString());
            }
        }

        public override string ToString()
        {
            return $"Ship {IMONumber}, Name: {Name}, Current position: {CurrentPosition}, Current load: {CurrentLoad}, "
                + $"Max load: {MaxLoad}, Length: {Length}, Width: {Width}";
        }
    }    
}
