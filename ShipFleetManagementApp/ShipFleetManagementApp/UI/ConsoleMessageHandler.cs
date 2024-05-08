using ShipFleetManagementApp.Backend;
using ShipFleetManagementApp.Backend.Utils;

namespace ShipFleetManagementApp.UI
{
    public static class ConsoleMessageHandler
    {
        private static readonly ShipownersManager _shipownersManager = new ShipownersManager();
        
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
                    Console.WriteLine("Invalid input. Please enter a number and click enter.");
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
        /// Shows the menu allowing to select a shipowner.
        /// </summary>
        public static void ShowShipownersSelectionMenu()
        {
            ShowMenuHeading();
            Console.WriteLine(
                "1. Add a shipowner.\n" +
                "2. Choose a shipowner.\n" +
                "3. Show shipowners.");
            ShowExitOption();
        }

        /// <summary>
        /// Shows the exit option for each menu.
        /// </summary>
        private static void ShowExitOption()
        {
            Console.WriteLine("-1. Exit.\n");
        }

        /// <summary>
        /// Performs a given task relating to shipowners.
        /// </summary>
        /// <param name="choice"></param>
        public static void ReactToShipownersChoice(int choice)
        {
            switch (choice)
            {
                case 1:
                    AddShipowner();
                    break;
                case 2:
                    ChooseShipowner();
                    break;
                case 3:
                    _shipownersManager.PrintShipowners();
                    break;
                default:
                    Console.WriteLine("Invalid input. Please enter a valid option.");
                    break;
            }
        }

        public static void ChooseShipowner()
        {
            Console.WriteLine("Choosing a shipowner.\nEnter the shipowner's number:");
            _shipownersManager.PrintShipowners();
            int choice = ReadNumericChoice();

            while (!_shipownersManager.IsShipownerIndexValid(choice))
            {
                Console.WriteLine("There's no such shipowner.\nEnter the shipowner's number:");
                _shipownersManager.PrintShipowners();
                choice = ReadNumericChoice();
            }
            
            Console.WriteLine($"You've chosen the shipowner {choice}: {_shipownersManager.Shipowners[choice]}");
            // TODO---------------------------------------------------------------------------------------
            // czy tu głębiej?
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
        /// Shows the message when the user wants to exit the program.
        /// </summary>
        public static void ShowExitMessage()
        {
            Console.WriteLine("You are now exiting the application. Thank you for using it, and see you soon!");
        }
    }
}
