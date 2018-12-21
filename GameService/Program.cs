using System;
using System.ServiceModel;

namespace GameService
{
    class Program
    {
        static void Main(string[] args)
        {
            // Database.SetInitializer(new DropCreateDatabaseAlways<GameContext>());
            ServiceHost selfHost = new ServiceHost(typeof(GameService));
            try
            {
                selfHost.Open();
                Console.WriteLine("The game service is running. Press X key to exit.");
                while (Console.ReadKey().Key != ConsoleKey.X)
                {                   
                }
                selfHost.Close();
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine("An exception occurred: {0}", ce.Message);
                selfHost.Abort();
            }
        }
    }
}
