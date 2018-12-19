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

        private static readonly PlayerStore _players = new PlayerStore();

        private IGameClient Client => OperationContext.Current.GetCallbackChannel<IGameClient>();
 
        public Player PlayerRegister(string name, string password)
        {
            // check if name and password are valid
            if (name.Length < PlayerNameMinLength && password.Length < PlayerPasswordMinLength)
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

        public Player PlayerLogIn(string name, string password)
        {
            using (var ctx = new GameContext())
            {
                return ctx.Players.Single(p => p.Name.Equals(name) && p.Password.Equals(password));
            }
        }

        public void PlayerConnect(Player player)
        {
            _players.AddPlayer(Client, player);
        }

        public void Broadcast(string message)
        {
            var me = _players.GetMe(Client);

            foreach (var player in _players.GetOnlinePlayers())
            {
                if (player != me)
                {
                    _players.GetGameClient(player).TestMethod(message);
                }
            }
        }
    }
}
