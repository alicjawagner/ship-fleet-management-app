using ShipFleetManagementApp.Backend.Ships.Cargo;

namespace ShipFleetManagementApp.Backend.Ships
{
    public class ContainerShip : Ship
    {
        private int _currentContainers = 0;

        public ContainerShip(string iMONumber, string name, double length, double width, double latitude, double longitude, double maxLoad, int maxContainers)
            : base(iMONumber, name, length, width, latitude, longitude, maxLoad)
        {
            if (maxContainers >= 0)
            {
                MaxContainers = maxContainers;
            }
            else
            {
                throw new ArgumentException($"MaxContainers {maxContainers} is incorrect. It should be a non-negative number.");
            }
        }

        /// <summary>
        /// Maximum permitted number of containers to be loaded onto the ship.
        /// </summary>
        public int MaxContainers { get; }

        /// <summary>
        /// List of containers loaded onto the ship.
        /// </summary>
        public List<Container> Containers { get; } = [];

        /// <summary>
        /// Current number of containers loaded onto the ship.
        /// </summary>
        public int CurrentContainers
        {
            get { return _currentContainers; }
            private set
            {
                if (value >= 0 && value <= MaxContainers)
                {
                    _currentContainers = value;
                }
                else
                {
                    throw new ArgumentException($"CurrentContainers {value} is incorrect. It should be less or equal to MaxContainers and a non-negative number.");
                }
            }
        }

        /// <summary>
        /// Loads the container onto the ship.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="addressee"></param>
        /// <param name="cargoDescription"></param>
        /// <param name="weight"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void LoadContainer(string sender, string addressee, string cargoDescription, double weight)
        {
            Containers.Add(new Container(sender, addressee, cargoDescription, weight));
            try
            {
                CurrentContainers++;
                try
                {
                    CurrentLoad += weight;
                }
                catch (ArgumentException)
                {
                    CurrentContainers--;
                    Containers.RemoveAt(Containers.Count - 1);
                    throw new InvalidOperationException("Loading container unsuccessful. The total weight exceeds the permitted load.");
                }
            }
            catch (ArgumentException)
            {
                Containers.RemoveAt(Containers.Count - 1);
                throw new InvalidOperationException("Loading container unsuccessful. The maximum number of containers has already been loaded.");
            }
        }

        /// <summary>
        /// Unloads the container at the given index on the list Containers from the ship.
        /// </summary>
        /// <param name="index"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void UnloadContainer(int index)
        {
            try
            {
                double weight = Containers[index].Weight;
                Containers.RemoveAt(index);
                CurrentContainers--;
                CurrentLoad -= weight;
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new InvalidOperationException("Unloading container unsuccessful. There is no container with this number.");
            }
        }

        /// <summary>
        /// Prints all the containers loaded onto the ship.
        /// </summary>
        public void PrintContainers()
        {
            if (Containers.Count == 0)
            {
                Console.WriteLine("There are no containers on this ship yet.");
                return;
            }

            Console.WriteLine($"Containers loaded onto {IMONumber} {Name}:");
            for (int i = 0; i < Containers.Count; i++)
            {
                Console.WriteLine($"Container {i}: {Containers[i]}");
            }
        }

        public override string ToString()
        {
            return "Container " + base.ToString();
        }
    }
}
