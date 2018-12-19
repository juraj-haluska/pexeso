using System.Collections.Generic;
using System.ServiceModel;
using GameService.Library;

namespace GameService
{
    public class PlayerStore
    {
        // lock is protecting _clients and _players
        private readonly object _lock = new object();
        private readonly List<IGameClient> _clients = new List<IGameClient>();
        private readonly List<Player> _players = new List<Player>();

        public void AddPlayer(IGameClient client, Player player)
        {
            if (client == null || player == null)
            {
                return;
            }

            lock (_lock)
            {
                _clients.Add(client);
                _players.Add(player);
            }
        }

        public List<Player> GetOnlinePlayers()
        {  
            var onlinePlayers = new List<Player>();

            lock (_lock)
            {
                for (var i = _clients.Count - 1; i >= 0; i--)
                {
                    var clientState = (ICommunicationObject) _clients[i];
                    if (clientState.State == CommunicationState.Opened)
                    {
                        onlinePlayers.Add(_players[i]);
                    }
                    else
                    {
                        _clients.RemoveAt(i);
                        _players.RemoveAt(i);
                    }
                }
            }

            return onlinePlayers;
        }

        public Player GetMe(IGameClient client)
        {
            lock (_lock)
            {
                var position = _clients.FindIndex(gameClient => gameClient == client);
                var clientState = (ICommunicationObject) _clients[position];

                if (clientState.State == CommunicationState.Opened)
                {
                    return _players[position];
                }

                _clients.RemoveAt(position);
                _players.RemoveAt(position);

                return null;
            }           
        }

        public IGameClient GetGameClient(Player player)
        {
            lock (_lock)
            {
                var position = _players.FindIndex(p => p == player);
                var clientState = (ICommunicationObject)_clients[position];

                if (clientState.State == CommunicationState.Opened)
                {
                    return _clients[position];
                }

                _clients.RemoveAt(position);
                _players.RemoveAt(position);

                return null;
            }
        }
    }
}
