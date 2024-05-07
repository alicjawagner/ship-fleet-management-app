using ShipFleetManagementApp.Backend.Utils;

namespace ShipFleetManagementApp.Backend.Ships
{
    public abstract class Ship
    {
        private string? _iMONumber;
        private double _length;
        private double _width;

        public string? Name { get; set; }
        public Coordinates CurrentPosition { get; protected set; }
        public List<LocationTimestamp> PositionHistory { get; }

        protected Ship()
        {
            PositionHistory = [];
        }

        protected Ship(string iMONumber, string name, double length, double width, double latitude, double longitude)
            : this()
        {
            IMONumber = iMONumber;
            Name = name;
            Length = length;
            Width = width;
            UpdatePosition(latitude, longitude);
        }

        public string IMONumber
        {
            get
            {
                return _iMONumber ?? "unspecified";
            }
            set
            {
                if (IsIMONumberCorrect(value))
                {
                    _iMONumber = value.Trim();
                }
                else
                {
                    throw new ArgumentException("IMO number is incorrect. It should start with capital letters 'IMO', followed by a space character, followed by 7 valid digits.");
                }
            }
        }

        public double Length
        {
            get { return _length; }
            set
            {
                if (value > 0)
                {
                    _length = value;
                }
                else
                {
                    throw new ArgumentException("Length is incorrect. It should be a positive number.");
                }
            }
        }

        public double Width
        {
            get { return _width; }
            set
            {
                if (value > 0)
                {
                    _width = value;
                }
                else
                {
                    throw new ArgumentException("Width is incorrect. It should be a positive number.");
                }
            }
        }

        public static bool IsIMONumberCorrect(string iMONumber)
        {
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

        public void UpdatePosition(double latitude, double longitude)
        {
            Coordinates position = new Coordinates(latitude, longitude);
            CurrentPosition = position;
            PositionHistory.Add(new LocationTimestamp(position));
        }
    }    
}
