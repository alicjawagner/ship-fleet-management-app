using ShipFleetManagementApp.Backend;
using ShipFleetManagementApp.Backend.Utils;
using System.Runtime.CompilerServices;

namespace ShipFleetManagementApp.UI
{
    public static class ConsoleMessageHandler
    {
        private static readonly ShipownersManager _shipownersManager = new ShipownersManager();
        private static Shipowner? _currentShipowner = null;
        
        /// <summary>
        /// Reads user's choice and casts it to int.
        /// </summary>
        /// <returns>The chosen number (int).</returns>
        /// <exception cref="ProgramTerminationException"></exception>
        public static int ReadNumericChoice()
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
                    Console.WriteLine("Invalid input. Please enter a number and press enter.");
                    input = Console.ReadLine();
                }
            }

            Console.WriteLine($"Your choice: {choice}");

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
                    int choice = ReadNumericChoice();
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
                    input = ReadNumericChoice();
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

            Console.WriteLine("Choosing a shipowner.\nEnter the shipowner's number:");
            _shipownersManager.PrintShipowners();
            int choice = ReadNumericChoice();

            while (!_shipownersManager.IsShipownerIndexValid(choice))
            {
                Console.WriteLine("There's no such shipowner.\nEnter the shipowner's number:");
                _shipownersManager.PrintShipowners();
                choice = ReadNumericChoice();
            }

            _currentShipowner = _shipownersManager.Shipowners[choice];
            return choice;
        }

        /// <summary>
        /// Performs adding a new shipowner.
        /// </summary>
        private static void AddShipowner()
        {
            Console.WriteLine("Adding a new shipowner.\nEnter the shipowner's name:");

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
            Console.WriteLine("Adding a new ship to the shipowner's list.\n" +
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
            int choice = ReadNumericChoice();
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
                        break;
                    case 2: // add tanker ship
                        //AddTankerShip();
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please enter a valid option.");
                        break;
                }

                if (choice != 0) // user doesn't go back, they stay in the menu
                {
                    ShowShipAddingMenu();
                    choice = ReadNumericChoice();
                }
            }
        }

        private static void AddContainerShip()
        {
            //TODO
            // string iMONumber, string name, double length, double width, double latitude, double longitude, double maxLoad, int maxContainers

            //_currentShipowner.AddContainerShip()
        }

        /// <summary>
        /// Shows the message when the user wants to exit the program.
        /// </summary>
        public static void ShowExitMessage()
        {
            Console.WriteLine("You are now exiting the application. Thank you for using it, and see you soon!");
        }
    }
}
