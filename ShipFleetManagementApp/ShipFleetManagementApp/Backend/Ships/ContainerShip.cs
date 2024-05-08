﻿using ShipFleetManagementApp.Backend.Ships.Cargo;
using ShipFleetManagementApp.Backend.Utils;

namespace ShipFleetManagementApp.Backend.Ships
{
    public class ContainerShip : Ship
    {
        private int _currentContainers = 0;
        public int MaxContainers { get; }
        public List<Container> Containers { get; } = [];

        public ContainerShip(string iMONumber, string name, double length, double width, double latitude, double longitude, double maxLoad, int maxContainers)
            : base(iMONumber, name, length, width, latitude, longitude, maxLoad)
        {
            if (maxContainers >= 0)
            {
                MaxContainers = maxContainers;
            }
            else
            {
                throw new ArgumentException("MaxContainers is incorrect. It should be a non-negative number.");
            }
        }

        public int CurrentContainers
        {
            get { return _currentContainers; }
            protected set
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

        public void PrintContainers()
        {
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
