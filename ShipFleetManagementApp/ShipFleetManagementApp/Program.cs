using ShipFleetManagementApp.Backend.Utils;
using CMH = ShipFleetManagementApp.UI.ConsoleMessageHandler;

namespace ShipFleetManagementApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int choice;
            bool stop = false;

            CMH.ShowWelcomeMessage();
            while (!stop)
            {
                try
                {
                    CMH.ShowInitialSelectionMenu();
                    choice = CMH.ReadNumericChoice();
                    CMH.ReactToInitialMenuChoice(choice);
                }
                catch (ProgramTerminationException)
                {
                    stop = true;
                    CMH.ShowExitMessage();
                }
            } 
        }
    }
}
