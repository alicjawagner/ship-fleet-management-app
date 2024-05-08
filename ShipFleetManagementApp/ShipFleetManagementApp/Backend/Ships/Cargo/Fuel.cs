namespace ShipFleetManagementApp.Backend.Ships.Cargo
{
    public class Fuel
    {
        private static Fuel? _dieselFuel = null;
        private static Fuel? _heavyFuel = null;

        private Fuel(string type, double density)
        {
            Type = type;
            Density = density;
        }

        public static Fuel Diesel
        {
            get
            {
                if (_dieselFuel == null)
                {
                    _dieselFuel = new Fuel("Diesel", 0.85);
                }
                return _dieselFuel;
            }
        }

        public static Fuel HeavyFuel
        {
            get
            {
                if (_heavyFuel == null)
                {
                    _heavyFuel = new Fuel("Heavy Fuel", 0.95);
                }
                return _heavyFuel;
            }
        }
        
        /// <summary>
        /// Name of fuel: Diesel or Heavy Fuel
        /// </summary>
        public string Type { get; }
        /// <summary>
        /// Density in kg/liter.
        /// </summary>
        public double Density { get; }
    }

}
