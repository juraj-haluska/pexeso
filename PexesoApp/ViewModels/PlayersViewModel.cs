using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using GameService.Library;

namespace PexesoApp.ViewModels
{
    class PlayersViewModel : Screen
    {
        private readonly IGameService _gameService;
        private readonly Player _me;
        private readonly GameEventHandler _eventHandler;
        private Player _selectedPlayer;

        public ObservableCollection<Player> Players { get; set; }

        public Player SelectedPlayer
        {
            get => _selectedPlayer;
            set
            {
                _selectedPlayer = value;
                NotifyOfPropertyChange(() => CanInvitePlayer);
            }
        }

        private void RegisterEvents()
        {
            _eventHandler.InvitedByEvent += (player, gameParams) =>
            {
                MessageBox.Show($"You were invited by player {player.Name} / {gameParams.GameSize}");
                _gameService.AcceptInvitation(player);
            };

            _eventHandler.NotifyPlayerConnectedEvent += player =>
            {
                Players.Add(player);
            };

            _eventHandler.NotifyPlayerDisconnectedEvent += player =>
            {
                var listedPlayer = Players.Single(p => p.Id == player.Id);
                Players.Remove(listedPlayer);
            };
        }

        public PlayersViewModel(IGameService gameService, GameEventHandler eventHandler, Player player)
        {
            _gameService = gameService;
            _eventHandler = eventHandler;
            _me = player;

            Players = new ObservableCollection<Player>(_gameService.GetAvailablePlayers().Where(p => p.Id != _me.Id));
            RegisterEvents();
        }

        public bool CanInvitePlayer => SelectedPlayer != null;

        public void InvitePlayer()
        {
            
        }

        protected override void OnDeactivate(bool close)
        {
            _gameService.DisconnectPlayer(_me);
            base.OnDeactivate(close);
        }
    }
}
