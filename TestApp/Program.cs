using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using GameService.Library;

namespace TestApp
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, UseSynchronizationContext = false)]
    class Program : IGameClient
    {
        static void Main(string[] args)
        {
            new Program().Run();
        }

        static Player RegisterPlayer(IGameService game)
        {
            Console.WriteLine("enter your name: ");
            var name = Console.ReadLine();
            Console.WriteLine("enter your password");
            var pass = Console.ReadLine();

            return game.RegisterPlayer(name, pass);
        }

        static Player ConnectPlayer (IGameService game)
        {
            Console.WriteLine("enter your name: ");
            var name = Console.ReadLine();
            Console.WriteLine("enter your password");
            var pass = Console.ReadLine();

            return game.ConnectPlayer(name, pass);
        }

        private IGameService _client;

        void Run()
        {
            var ctx = new InstanceContext(this);
            using (var channelFactory = new DuplexChannelFactory<IGameService>(ctx, "GameServiceEndPoint"))
            {
                _client = channelFactory.CreateChannel();
                
                while (Console.ReadKey().Key != ConsoleKey.X)
                {
                    Console.WriteLine("Enter command:");

                    var cmd = Console.ReadKey().Key;

                    if (cmd == ConsoleKey.A)
                    {
                        var registered = RegisterPlayer(_client);

                        if (registered != null)
                        {
                            Console.WriteLine("Player has been registered. ID: " + registered.Id);
                        }
                        else
                        {
                            Console.WriteLine("Player registration error!");
                        }
                    }
                    else if (cmd == ConsoleKey.B)
                    {
                        var player = ConnectPlayer(_client);

                        if (player != null)
                        {
                            Console.WriteLine("Player was connected");
                        }
                        else
                        {
                            Console.WriteLine("Player wasn't connected!");
                        }
                    }
                    else if (cmd == ConsoleKey.C)
                    {
                        var players = _client.GetAvailablePlayers();

                        Console.WriteLine("Online players: ");

                        foreach (var player in players)
                        {
                            Console.WriteLine($"name: {player.Name} id: {player.Id}");
                        }
                    }
                    else if (cmd == ConsoleKey.I)
                    {
                        Console.WriteLine("Type id of player to invite: ");
                        int id;
                        int.TryParse(Console.ReadLine(), out id);

                        var player = new Player();
                        player.Id = id;

                        _client.InvitePlayer(player, GameParams.GameSizes.Size4X3);
                    }
                }
            }
        }

        public void InvitedBy(Player player, GameParams.GameSizes gameSize)
        {
            Console.WriteLine($"You were invited by: {player.Name}, id: {player.Id}");
            Console.WriteLine(gameSize);

            Console.WriteLine("accept? y/n");
            var key = Console.ReadKey().Key;
            if (key == ConsoleKey.Y)
            {
                _client.AcceptInvitation(player, gameSize);
            }
            else
            {
                _client.RefuseInvitation(player);
            }
        }

        public void StartGame(GameParams gameParams)
        {
            //Console.WriteLine($"Invitation accepted by: {player.Name}, id: {player.Id}");
        }

        public void InvitationRefused(Player player)
        {
            Console.WriteLine($"Invitation refused by: {player.Name}, id: {player.Id}");
        }

        public void NotifyPlayerConnected(Player player)
        {
            //throw new NotImplementedException();
        }

        public void NotifyPlayerDisconnected(Player player)
        {
            //throw new NotImplementedException();
        }

        public void NotifyPlayerUpdate(Player player)
        {
            throw new NotImplementedException();
        }


    }
}
