using ShipFleetManagementApp.Backend;
using ShipFleetManagementApp.Backend.Ships;
using ShipFleetManagementApp.Backend.Ships.Cargo;
using ShipFleetManagementApp.Backend.Utils;
using System.Globalization;

namespace ShipFleetManagementApp.UI
{
    public static class ConsoleMessageHandler
    {
        private static readonly ShipownersManager _shipownersManager = new ShipownersManager();
        private static Shipowner? _currentShipowner = null;
        private static Ship? _currentShip = null;
        private static ContainerShip? _currentContainerShip = null;
        private static TankerShip? _currentTankerShip = null;
        /// <summary>true - it is a container ship, false - it is a tanker ship</summary>
        private static bool _isContainerShip = false;
        private static Tank? _currentTank = null;
        
        /// <summary>
        /// Reads user's input and casts it to int.
        /// </summary>
        /// <returns>The chosen number (int).</returns>
        /// <exception cref="ProgramTerminationException"></exception>
        public static int ReadIntInput()
        {
            string? input = Console.ReadLine();
            int choice = -1;
            bool success = false;

            while (!success)
            {
                if (int.TryParse(input, out int result))
                {
                    choice = result;
                    success = true;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number (integer) and press enter.");
                    input = Console.ReadLine();
                }
            }

            if (choice == -1)
            {
                throw new ProgramTerminationException();
            }
            return choice;
        }

        /// <summary>
        /// Shows welcome message. Should only be used once, at the beginning.
        /// </summary>
        public static void ShowWelcomeMessage()
        {
            Console.WriteLine("Welcome to the Ship Fleet Management Application!\n");
            Console.WriteLine("Here you can manage your fleet, add shipowners and ships.\n");
        }

        /// <summary>
        /// Shows the message when the user wants to exit the program.
        /// </summary>
        public static void ShowExitMessage()
        {
            Console.WriteLine("\nYou are now exiting the application. Thank you for using it, and see you soon!");
        }

        /// <summary>
        /// Shows the initial menu allowing to add/select/show a shipowner.
        /// </summary>
        public static void ShowInitialSelectionMenu()
        {
            _currentShipowner = null;

            ShowMenuHeading();
            Console.WriteLine(
                "1. Add a shipowner.\n" +
                "2. Choose a shipowner.\n" +
                "3. Show shipowners.");
            ShowExitOption();
        }

        /// <summary>
        /// Performs a given task related to adding/choosing/showing shipowners.
        /// </summary>
        /// <param name="input"></param>
        public static void ReactToInitialMenuChoice(int input)
        {
            switch (input)
            {
                case 1: // add shipowner
                    AddShipowner();
                    break;

                case 2: // choose shipowner
                    int shipownerIndex = ChooseShipowner();
                    if (shipownerIndex == -1) { break; }
                    ShowShipownersMenu();
                    int choice = ReadIntInput();
                    ReactToShipownersMenuChoice(choice);
                    break;

                case 3: // print shipowners
                    _shipownersManager.PrintShipowners();
                    break;

                default:
                    Console.WriteLine("Invalid input. Please enter a valid option.");
                    break;
            }
        }

        /// <summary>
        /// Reads user's input and casts it to double.
        /// </summary>
        /// <returns>The chosen number (double).</returns>
        private static double ReadDoubleInput()
        {
            string input = Console.ReadLine() ?? "";
            double choice = -1;
            bool success = false;

            while (!success)
            {
                input = input.Replace(",", ".");
                if (double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
                {
                    choice = result;
                    success = true;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number (e.g. 20.5) and press enter.");
                    input = Console.ReadLine() ?? "";
                }
            }
            return choice;
        }

        /// <summary>
        /// Shows the heading for each menu (Choose what would you like to do)
        /// </summary>
        private static void ShowMenuHeading()
        {
            Console.WriteLine("\nChoose what would you like to do (enter the correct number and click enter):");
        }

        /// <summary>
        /// Shows the exit option for each menu.
        /// </summary>
        private static void ShowExitOption()
        {
            Console.WriteLine("-1. Exit the program.\n");
        }

        /// <summary>
        /// Shows the menu allowing to add/select a ship / show ships of the shipowner.
        /// </summary>
        private static void ShowShipownersMenu()
        {
            _currentShip = null;
            _currentContainerShip = null;
            _currentTankerShip = null;
            _isContainerShip = false;

            ShowMenuHeading();
            Console.WriteLine($"You've chosen the shipowner: {_currentShipowner}");
            Console.WriteLine(
                "0. Go back.\n" +
                "1. Add a ship to the shipowner's list.\n" +
                "2. Choose a ship from the shipowner's list.\n" +
                "3. Show the shipowner's ships.");
            ShowExitOption();
        }

        /// <summary>
        /// Shows the menu allowing to choose the type of the ship to be added.
        /// </summary>
        private static void ShowShipAddingMenu()
        {
            Console.WriteLine("\nAdding a new ship to the shipowner's list.\n" +
                "Choose the type of the ship:\n" +
                "0. Go back.\n" +
                "1. Container ship.\n" +
                "2. Tanker ship.");
            ShowExitOption();
        }

        /// <summary>
        /// Shows the menu allowing to update position / show position history of the ship
        /// and do tasks specific for the type of the ship:
        /// <para>container ship - show containers, load/unload container</para>
        /// <para>tanker ship - show tanks, refuel/empty tank</para>
        /// </summary>
        private static void ShowShipsMenu()
        {
            _currentTank = null;

            ShowMenuHeading();
            Console.WriteLine($"You've chosen the ship: {_currentShip}");
            Console.WriteLine(
                "0. Go back.\n" +
                "1. Update the position of the ship.\n" +
                "2. Show the position history of the ship.");
            if ( _isContainerShip )
            {
                Console.WriteLine(
                    "3. Load a container.\n" +
                    "4. Unload a container.\n" +
                    "5. Show containers loaded onto this ship.");
            }
            else
            {
                Console.WriteLine(
                    "3. Choose a tank.\n" +
                    "4. Show tanks installed on this ship.");
            }
            ShowExitOption();
        }

        /// <summary>
        /// Show the menu for specifying the configuration of tanks on this ship.
        /// </summary>
        private static void ShowTanksAddingMenu()
        {
            Console.WriteLine("\nPlease specify the configuration of tanks for this ship.");
            Console.WriteLine("You can add tanks one by one or add several tanks of the same type at once. Choose what would you like to do:");
            Console.WriteLine(
                "1. Add a tank.\n" +
                "2. Add several tanks at once.\n" +
                "3. Finish adding tanks.");
            ShowExitOption();
        }

        /// <summary>
        /// Shows the menu allowing to refuel/empty the tank.
        /// </summary>
        private static void ShowTanksMenu()
        {
            ShowMenuHeading();
            Console.WriteLine($"You've chosen the tank: {_currentTank}");
            Console.WriteLine(
                "0. Go back.\n" +
                "1. Refuel the tank.\n" +
                "2. Empty the tank.");
            ShowExitOption();
        }

        /// <summary>
        /// Shows the menu for choosing the value to be changed (during creating a ship).
        /// </summary>
        private static void ShowShipValuesChangeMenu()
        {
            Console.WriteLine("Please choose the number of the value you'd like to change.");
            Console.WriteLine(
                "1. IMO Number\n" +
                "2. Name\n" +
                "3. Length\n" +
                "4. Width\n" +
                "5. Latitude\n" +
                "6. Longitude\n" +
                "7. Maximum load");
        }

        /// <summary>
        /// Shows the menu for choosing the value to be changed (during updating the ship position).
        /// </summary>
        private static void ShowPositionUpdateValuesChangeMenu()
        {
            Console.WriteLine("Please choose the number of the value you'd like to change.");
            Console.WriteLine(
                "1. Latitude\n" +
                "2. Longitude");
        }

        /// <summary>
        /// Performs a given task related to adding/choosing/printing ships.
        /// </summary>
        /// <param name="input"></param>
        private static void ReactToShipownersMenuChoice(int input)
        {
            bool stop = false;

            while (!stop)
            {
                switch (input)
                {
                    case 0: //go back
                        stop = true;
                        break;
                    case 1: // add ship
                        AddShip();
                        break;
                    case 2: // choose ship
                        int shipIndex = ChooseShip();
                        if (shipIndex == -1) { break; }
                        ShowShipsMenu();
                        int choice = ReadIntInput();
                        ReactToShipsMenuChoice(choice);
                        break;
                    case 3: // print ships
                        _currentShipowner!.PrintShips();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please enter a valid option.");
                        break;
                }

                if (input != 0) // user doesn't go back, they stay in the menu
                {
                    ShowShipownersMenu();
                    input = ReadIntInput();
                }
            }
        }

        /// <summary>
        /// Performs a given task: updating ship's position, printing position history, actions with containers/tanks.
        /// </summary>
        /// <param name="input"></param>
        private static void ReactToShipsMenuChoice(int input)
        {
            bool stop = false;

            while (!stop)
            {
                switch (input)
                {
                    case 0: //go back
                        stop = true;
                        break;

                    case 1: // position update
                        UpdateShipPosition();
                        break;

                    case 2: // show position history
                        _currentShip!.PrintPositionHistory();
                        break;

                    case 3: // load container / choose tank
                        if (_isContainerShip)
                            LoadContainer();
                        else
                        {
                            int choice = ChooseTank();
                            if (choice == -1) break;
                            ShowTanksMenu();
                            choice = ReadIntInput();
                            ReactToTanksMenuChoice(choice);
                        } 
                        break;

                    case 4: // unload container / show tanks
                        if (_isContainerShip)
                            UnloadContainer();
                        else
                            _currentTankerShip!.PrintTanks();
                        break;

                    case 5: // show containers / invalid input
                        if (_isContainerShip)
                            _currentContainerShip!.PrintContainers();
                        else
                            Console.WriteLine("Invalid input. Please enter a valid option.");
                        break;

                    default:
                        Console.WriteLine("Invalid input. Please enter a valid option.");
                        break;
                }

                if (input != 0) // user doesn't go back, they stay in the menu
                {
                    ShowShipsMenu();
                    input = ReadIntInput();
                }
            }
        }

        /// <summary>
        /// Performs a given task: refueling/emptying tank.
        /// </summary>
        /// <param name="input"></param>
        private static void ReactToTanksMenuChoice(int input)
        {
            bool stop = false;

            while (!stop)
            {
                switch (input)
                {
                    case 0: // go back
                        stop = true;
                        break;
                    case 1: // refuel tank
                        RefuelTank();
                        input = 0;
                        stop = true;
                        break;
                    case 2: // empty tank
                        EmptyTank();
                        input = 0;
                        stop = true;
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please enter a valid option.");
                        break;
                }

                if (input != 0) // user doesn't go back, they stay in the menu
                {
                    ShowTanksMenu();
                    input = ReadIntInput();
                }
            }
        }

        /// <summary>
        /// Performs adding a new shipowner.
        /// </summary>
        private static void AddShipowner()
        {
            Console.WriteLine("\nAdding a new shipowner.\nEnter the shipowner's name:");

            string? name = Console.ReadLine();
            while (name == null)
            {
                Console.WriteLine("Enter the shipowner's name:");
                name = Console.ReadLine();
            }
            _shipownersManager.AddShipowner(new Shipowner(name));
            Console.WriteLine($"Shipowner named {name} has been added successfully.");
        }

        /// <summary>
        /// Performs choosing a shipowner from the list.
        /// </summary>
        /// <returns>The index of the chosen shipowner or -1 if no shipowner chosen.</returns>
        private static int ChooseShipowner()
        {
            if (_shipownersManager.Shipowners.Count == 0)
            {
                Console.WriteLine("There are no shipowners to choose from yet. First add a shipowner.");
                return -1;
            }

            Console.WriteLine("\nChoosing a shipowner.\nEnter the shipowner's number:");
            _shipownersManager.PrintShipowners();
            int choice = ReadIntInput();

            while (!_shipownersManager.IsShipownerIndexValid(choice))
            {
                Console.WriteLine("There's no such shipowner.\nEnter the shipowner's number:");
                _shipownersManager.PrintShipowners();
                choice = ReadIntInput();
            }

            _currentShipowner = _shipownersManager.Shipowners[choice];
            return choice;
        }

        /// <summary>
        /// Performs the process of selection the ship type.
        /// </summary>
        private static void AddShip()
        {
            ShowShipAddingMenu();
            int choice = ReadIntInput();
            bool stop = false;

            while (!stop)
            {
                switch (choice)
                {
                    case 0: //go back
                        stop = true;
                        break;
                    case 1: // add container ship
                        AddContainerShip();
                        choice = 0;
                        break;
                    case 2: // add tanker ship
                        AddTankerShip();
                        choice = 0;
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please enter a valid option.");
                        break;
                }

                if (choice != 0) // user doesn't go back, they stay in the menu
                {
                    ShowShipAddingMenu();
                    choice = ReadIntInput();
                }
            }
        }

        /// <summary>
        /// Performs choosing a ship from the list.
        /// </summary>
        /// <returns>The index of the chosen ship or -1 if no ship chosen.</returns>
        private static int ChooseShip()
        {
            if (_currentShipowner!.Ships.Count == 0)
            {
                Console.WriteLine("There are no ships to choose from yet. First add a ship.");
                return -1;
            }

            Console.WriteLine("\nChoosing a ship.\nEnter the ship's number:");
            _currentShipowner.PrintShips();
            int choice = ReadIntInput();

            while (!_currentShipowner.IsShipIndexValid(choice))
            {
                Console.WriteLine("There's no such ship.\nEnter the ship's number:");
                _currentShipowner.PrintShips();
                choice = ReadIntInput();
            }

            _currentShip = _currentShipowner.Ships[choice];
            if (_currentShip is ContainerShip)
            {
                _isContainerShip = true;
                _currentContainerShip = (ContainerShip)_currentShip;
            }
            else
            {
                _isContainerShip = false;
                _currentTankerShip = (TankerShip)_currentShip;
            }
            return choice;
        }

        /// <summary>
        /// Performs adding a container ship to the shipowner's ship list.
        /// </summary>
        private static void AddContainerShip()
        {
            string iMONumber, name;
            double length, width, latitude, longitude, maxLoad;
            int maxContainers;
            bool success = false;

            Console.WriteLine("\nYou've chosen to add a new container ship. Please provide the following information.");
            Console.Write("IMO Number (format: IMO 1234567): ");
            iMONumber = Console.ReadLine() ?? "unspecified";
            Console.Write("Name: ");
            name = Console.ReadLine() ?? "unspecified";
            Console.Write("Length in meters: ");
            length = ReadDoubleInput();
            Console.Write("Width in meters: ");
            width = ReadDoubleInput();
            Console.Write("Position - latitude: ");
            latitude = ReadDoubleInput();
            Console.Write("Position - longitude: ");
            longitude = ReadDoubleInput();
            Console.Write("Maximum permitted load in tons: ");
            maxLoad = ReadDoubleInput();
            Console.Write("Maximum permitted number of containers: ");
            maxContainers = ReadIntInput();

            while (!success)
            {
                try
                {
                    Console.WriteLine("\nAdding the ship...");
                    _currentShipowner!.AddContainerShip(iMONumber, name, length, width, latitude, longitude, maxLoad, maxContainers);
                    Console.WriteLine("The ship has been added successfully.");
                    success = true;
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                    ShowShipValuesChangeMenu();
                    Console.WriteLine("8. Maximum number of containers");
                    int input = ReadIntInput();
                    Console.Write("New value: ");
                    switch (input)
                    {
                        case 1:
                            iMONumber = Console.ReadLine() ?? "unspecified";
                            break;
                        case 2:
                            name = Console.ReadLine() ?? "unspecified";
                            break;
                        case 3:
                            length = ReadDoubleInput();
                            break;
                        case 4:
                            width = ReadDoubleInput();
                            break;
                        case 5:
                            latitude = ReadDoubleInput();
                            break;
                        case 6:
                            longitude = ReadDoubleInput();
                            break;
                        case 7:
                            maxLoad = ReadDoubleInput();
                            break;
                        case 8:
                            maxContainers = ReadIntInput();
                            break;
                        default:
                            Console.WriteLine("Invalid number. Try again.");
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Performs adding a tanker ship to the shipowner's ship list.
        /// </summary>
        private static void AddTankerShip()
        {
            TankerShip? tankerShip = null;
            string iMONumber, name;
            double length, width, latitude, longitude, maxLoad;
            bool success = false;

            Console.WriteLine("\nYou've chosen to add a new tanker ship. Please provide the following information.");
            Console.Write("IMO Number (format: IMO 1234567): ");
            iMONumber = Console.ReadLine() ?? "unspecified";
            Console.Write("Name: ");
            name = Console.ReadLine() ?? "unspecified";
            Console.Write("Length in meters: ");
            length = ReadDoubleInput();
            Console.Write("Width in meters: ");
            width = ReadDoubleInput();
            Console.Write("Position - latitude: ");
            latitude = ReadDoubleInput();
            Console.Write("Position - longitude: ");
            longitude = ReadDoubleInput();
            Console.Write("Maximum permitted load in tons: ");
            maxLoad = ReadDoubleInput();

            while (!success)
            {
                try
                {
                    Console.WriteLine("\nAdding the ship...");
                    tankerShip = _currentShipowner!.AddTankerShip(iMONumber, name, length, width, latitude, longitude, maxLoad);
                    Console.WriteLine("The ship has been added successfully.");
                    success = true;
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                    ShowShipValuesChangeMenu();
                    int input = ReadIntInput();
                    Console.Write("New value: ");
                    switch (input)
                    {
                        case 1:
                            iMONumber = Console.ReadLine() ?? "unspecified";
                            break;
                        case 2:
                            name = Console.ReadLine() ?? "unspecified";
                            break;
                        case 3:
                            length = ReadDoubleInput();
                            break;
                        case 4:
                            width = ReadDoubleInput();
                            break;
                        case 5:
                            latitude = ReadDoubleInput();
                            break;
                        case 6:
                            longitude = ReadDoubleInput();
                            break;
                        case 7:
                            maxLoad = ReadDoubleInput();
                            break;
                        default:
                            Console.WriteLine("Invalid number. Try again.");
                            break;
                    }
                }
            }
            ConfigureTanks(tankerShip!);
        }

        /// <summary>
        /// Performs the process of configuring the tanks on a tanker ship.
        /// </summary>
        /// <param name="tankerShip"></param>
        private static void ConfigureTanks(TankerShip tankerShip)
        {
            ShowTanksAddingMenu();
            int choice = ReadIntInput();
            List<Tank> tanks = [];
            bool stop = false;

            while (!stop)
            {
                switch (choice)
                {
                    case 1: // add one tank
                        tanks.Add(CreateOneTank());
                        break;
                    case 2: // add several tanks
                        tanks.AddRange(CreateSeveralTanks());
                        break;
                    case 3: // finish adding tanks
                        stop = true;
                        Console.WriteLine("All tanks have been saved successfully.");
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please enter a valid option.");
                        break;
                }

                if (choice != 3) // user doesn't finish, they stay in the menu
                {
                    ShowTanksAddingMenu();
                    choice = ReadIntInput();
                }
            }
            tankerShip.Tanks = tanks;
        }

        /// <summary>
        /// Performs creating a new tank to be added to the tanker ship.
        /// </summary>
        /// <returns>Created tank object.</returns>
        private static Tank CreateOneTank()
        {
            double maxCapacity;
            Tank? tank = null;
            bool success = false;

            Console.WriteLine("\nYou are adding one tank. Please provide the following information.");
            Console.Write("Maximum capacity in liters: ");
            maxCapacity = ReadDoubleInput();

            while (!success)
            {
                try
                {
                    Console.WriteLine("\nAdding the tank...");
                    tank = new Tank(maxCapacity);
                    Console.WriteLine("The tank has been added successfully.");
                    success = true;
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                    Console.Write("Enter the new value for maximum capacity: ");
                    maxCapacity = ReadDoubleInput();
                }
            }

            return tank!;
        }

        /// <summary>
        /// Performs creating several tanks with the same parameters to be added to the tanker ship.
        /// </summary>
        /// <returns>The list of created tanks.</returns>
        private static List<Tank> CreateSeveralTanks()
        {
            double maxCapacity;
            int tanksNumber;
            List<Tank> tanks = [];
            bool success = false;

            Console.WriteLine("\nYou are adding several tanks with the same parameters. Please provide the following information.");
            Console.Write("Maximum tank capacity in liters: ");
            maxCapacity = ReadDoubleInput();
            Console.Write("How many tanks do you want to add: ");
            tanksNumber = ReadIntInput();

            while (!success)
            {
                try
                {
                    Console.WriteLine("\nAdding the tanks...");
                    for (int i = 0; i < tanksNumber; i++)
                    {
                        tanks.Add(new Tank(maxCapacity));
                    }
                    Console.WriteLine("The tanks have been added successfully.");
                    success = true;
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                    Console.Write("Enter the new value for maximum capacity: ");
                    maxCapacity = ReadDoubleInput();
                }
            }

            return tanks;
        }

        /// <summary>
        /// Performs the process of updating the ship position.
        /// </summary>
        private static void UpdateShipPosition()
        {
            double latitude, longitude;
            bool success = false;

            Console.WriteLine("\nYou've chosen to update the position of the ship. Please provide the following information.");
            Console.Write("Latitude: ");
            latitude = ReadDoubleInput();
            Console.Write("Longitude: ");
            longitude = ReadDoubleInput();

            while (!success)
            {
                try
                {
                    Console.WriteLine("\nUpdating the position...");
                    _currentShip!.UpdatePosition(latitude, longitude);
                    Console.WriteLine("The position has been updated successfully.");
                    success = true;
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                    ShowPositionUpdateValuesChangeMenu();
                    int input = ReadIntInput();
                    Console.Write("New value: ");
                    switch (input)
                    {
                        case 1:
                            latitude = ReadDoubleInput();
                            break;
                        case 2:
                            longitude = ReadDoubleInput();
                            break;
                        default:
                            Console.WriteLine("Invalid number. Try again.");
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Performs the process of loading a container onto the ship.
        /// </summary>
        private static void LoadContainer()
        {
            string sender, addressee, cargoDescription;
            double weight;
            bool success = false;

            Console.WriteLine("\nYou are loading a container. Please provide the following information.");
            Console.Write("Sender: ");
            sender = Console.ReadLine() ?? "unspecified";
            Console.Write("Addressee: ");
            addressee = Console.ReadLine() ?? "unspecified";
            Console.Write("Cargo description: ");
            cargoDescription = Console.ReadLine() ?? "unspecified";
            Console.Write("Weight in tons: ");
            weight = ReadDoubleInput();

            while (!success)
            {
                try
                {
                    Console.WriteLine("\nLoading the container...");
                    _currentContainerShip!.LoadContainer(sender, addressee, cargoDescription, weight);
                    Console.WriteLine("The container has been loaded successfully.");
                    success = true;
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                    Console.Write("Enter the new value for weight: ");
                    weight = ReadDoubleInput();
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e.Message);
                    success = true;
                }
            }
        }

        /// <summary>
        /// Performs the process of unloading a container.
        /// </summary>
        private static void UnloadContainer()
        {
            if (_currentContainerShip!.Containers.Count == 0)
            {
                Console.WriteLine("There are no containers to unload.");
                return;
            }

            Console.WriteLine("\nYou are unloading a container. Please select the container you'd like to unload. Enter the container's number and press enter.");
            _currentContainerShip.PrintContainers();
            int input = ReadIntInput();
            Console.WriteLine("\nUnloading the container...");

            try
            {
                _currentContainerShip!.UnloadContainer(input);
                Console.WriteLine("The container has been unloaded successfully.");
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Performs the process of choosing a tank.
        /// </summary>
        /// <returns>The index of the chosen tank or -1 if no tank chosen.</returns>
        private static int ChooseTank()
        {
            if (_currentTankerShip!.Tanks.Count == 0)
            {
                Console.WriteLine("There are no tanks installed on this ship.");
                return -1;
            }

            Console.WriteLine("\nChoosing a tank.\nEnter the tank's number:");
            _currentTankerShip.PrintTanks();
            int choice = ReadIntInput();

            if (!_currentTankerShip.IsTankIndexValid(choice))
            {
                Console.WriteLine("There's no such tank.");
                return -1;
            }

            _currentTank = _currentTankerShip.Tanks[choice];
            return choice;
        }

        /// <summary>
        /// Performs the process of refueling the tank.
        /// </summary>
        private static void RefuelTank()
        {
            Fuel fuelType;
            int fuelChoice;
            double liters;

            Console.WriteLine("\nYou are refueling the tank. Please provide the following information.");

            Console.WriteLine("Fuel type:\n" +
                "1. Diesel\n" +
                "2. Heavy fuel");
            fuelChoice = ReadIntInput();
            while (fuelChoice != 1 && fuelChoice != 2)
            {
                Console.WriteLine("Incorrect number. Try again.");
                fuelChoice = ReadIntInput();
            }
            fuelType = fuelChoice == 1 ? Fuel.Diesel : Fuel.HeavyFuel;

            Console.Write("How many liters you want to refuel: ");
            liters = ReadDoubleInput();

            try
            {
                Console.WriteLine("\nRefueling the tank...");
                _currentTank!.RefuelTank(fuelType, liters);
                Console.WriteLine("The tank has been refueled successfully.");
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Performs the process of emptying the tank.
        /// </summary>
        private static void EmptyTank()
        {
            Console.WriteLine("Emptying the tank...");
            try
            {
                _currentTank!.EmptyTank();
                Console.WriteLine("The tank has been emptied successfully.");
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
