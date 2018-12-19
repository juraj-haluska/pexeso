using System;
using System.Collections.Generic;
using System.ServiceModel;
using GameService.Library;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            InstanceContext ctx = new InstanceContext(new ClientCode());

            using (var channelFactory = new DuplexChannelFactory<IGameService>(ctx, "GameServiceEndPoint"))
            {
                var client = channelFactory.CreateChannel();

                var p = new Player("hello", "player");

                client.PlayerConnect(p);

                while(Console.ReadKey().Key != ConsoleKey.S)
                { }

                client.Broadcast("ahoj");

                Console.ReadKey();
            }
        }
    }
}
