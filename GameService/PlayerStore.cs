using System.Collections.Generic;
using System.ServiceModel;
using GameService.Library;

namespace GameService
{
    // this class maps and manages connections with players
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

        public IGameClient RemovePlayer(Player player)
        {
            lock (_lock)
            {
                var index = _players.FindIndex(p => p.Id == player.Id);
                _players.RemoveAt(index);
                var gameClient = _clients[index];
                _clients.RemoveAt(index);
                return gameClient;
            }
        }

        public List<Player> GetActivePlayers()
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

        public Player GetGamePlayer(IGameClient client)
        {
            lock (_lock)
            {
                var position = _clients.FindIndex(gameClient => gameClient == client);

                if (position < 0)
                {
                    return null;
                }

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
                var position = _players.FindIndex(p => p.Id == player.Id);
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
