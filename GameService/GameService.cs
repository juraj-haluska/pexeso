using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using GameService.Data;
using GameService.Library;

namespace GameService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    class GameService : IGameService
    {
        private const int PlayerNameMinLength = 5;
        private const int PlayerPasswordMinLength = 5;

        // holds players available for game
        private static readonly PlayerStore PlayersOnline = new PlayerStore();
    
        // holds players currently in game
        private static readonly PlayerStore PlayersInGame = new PlayerStore();

        private IGameClient Client => OperationContext.Current.GetCallbackChannel<IGameClient>();
 
        public Player PlayerRegister(string name, string password)
        {
            // check if name and password are valid
            if (name.Length < PlayerNameMinLength || password.Length < PlayerPasswordMinLength)
            {
                return null;
            }

            using (var ctx = new GameContext())
            {
                // check if player's name is unique
                var count = ctx.Players.Count(p => p.Name.Equals(name));
                if (count != 0)
                {
                    return null;
                }

                var newPlayer = ctx.Players.Add(new Player(name, password));
                ctx.SaveChanges();

                return newPlayer;
            }
        }

        public Player PlayerConnect(string name, string password)
        {
            var ctx = new GameContext();

            try
            {
                var player = ctx.Players.Single(p => p.Name.Equals(name) && p.Password.Equals(password));
                PlayersOnline.AddPlayer(Client, player);
                return player;
            }
            catch
            {
                return null;
            }
            finally
            {
                ctx.Dispose();
            }      
        }

        public List<Player> GetAvailablePlayers()
        {
            return PlayersOnline.GetActivePlayers();
        }

        public void InvitePlayer(Player player, GameParams gameParams)
        {
            var invitingPlayer = PlayersOnline.GetGamePlayer(Client);
            IGameClient client = PlayersOnline.GetGameClient(player);
            client?.InvitedBy(invitingPlayer, gameParams);
        }

        public void AcceptInvitation(Player player)
        {
            var acceptingPlayer = PlayersOnline.GetGamePlayer(Client);
            IGameClient client = PlayersOnline.GetGameClient(player);
            client?.InvitationAccepted(acceptingPlayer);
        }

        public void RefuseInvitation(Player player)
        {
            var refusingPlayer = PlayersOnline.GetGamePlayer(Client);
            IGameClient client = PlayersOnline.GetGameClient(player);
            client?.InvitationRefused(refusingPlayer);
        }
    }
}
