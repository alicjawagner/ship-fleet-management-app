using ShipFleetManagementApp.Backend;
using ShipFleetManagementApp.Backend.Utils;
using System.Globalization;

namespace ShipFleetManagementApp.UI
{
    public static class ConsoleMessageHandler
    {
        private static readonly ShipownersManager _shipownersManager = new ShipownersManager();
        private static Shipowner? _currentShipowner = null;
        
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
        /// Shows welcome message. Should only be used once, at the beginning.
        /// </summary>
        public static void ShowWelcomeMessage()
        {
            Console.WriteLine("Welcome to the Ship Fleet Management Application!\n");
            Console.WriteLine("Here you can manage your fleet, add shipowners and ships.\n");
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
            Console.WriteLine("-1. Exit.\n");
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
        /// Shows the menu allowing to add/select a ship / show ships of the shipowner.
        /// </summary>
        private static void ShowShipownersMenu()
        {
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
                        // TODO
                        //----------------------------------------------------------------------------------------------------------------------------
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
        /// Performs adding a new shipowner.
        /// </summary>
        private static void AddShipowner()
        {
            Console.WriteLine("\nAdding a new shipowner.\nEnter the shipowner's name:");

            string? name = Console.ReadLine();
            while (name == null) {
                Console.WriteLine("Enter the shipowner's name:");
                name = Console.ReadLine();
            }
            _shipownersManager.AddShipowner(new Shipowner(name));
            Console.WriteLine($"Shipowner named {name} has been added successfully.");
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
        /// Performs adding a container ship to the shipowner's ship list.
        /// </summary>
        private static void AddContainerShip()
        {
            string iMONumber, name;
            double length, width, latitude, longitude, maxLoad;
            int maxContainers;
            bool success = false;

            Console.WriteLine("\nYou've chosen to add a new container ship. Please provide the information below.");
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
                    success = true;
                    Console.WriteLine("The ship has been added successfully.");
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
            string iMONumber, name;
            double length, width, latitude, longitude, maxLoad;
            bool success = false;

            Console.WriteLine("\nYou've chosen to add a new tanker ship. Please provide the information below.");
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
                    _currentShipowner!.AddTankerShip(iMONumber, name, length, width, latitude, longitude, maxLoad);
                    success = true;
                    Console.WriteLine("The ship has been added successfully.");
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
            //TODO add tanks
        }

        /// <summary>
        /// Shows the menu for choosing the value to be changed.
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
        /// Shows the message when the user wants to exit the program.
        /// </summary>
        public static void ShowExitMessage()
        {
            Console.WriteLine("\nYou are now exiting the application. Thank you for using it, and see you soon!");
        }
    }
}
