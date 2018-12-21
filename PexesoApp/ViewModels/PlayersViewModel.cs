using System;
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

        public Action<GameParams, Player> GameCreated { get; set; }

        public ObservableCollection<GameTypeViewModel> GameTypes { get; set; }

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

        public GameTypeViewModel SelectedGameType { get; set; }

        private void RegisterEvents()
        {
            _eventHandler.InvitedByEvent += (player, gameSize) =>
            {
                var message = MessageBox.Show($"You were invited by player {player.Name} / {gameSize}", "Invitation",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (message == MessageBoxResult.Yes)
                {
                    _gameService.AcceptInvitation(player, gameSize);
                }
                else
                {
                    _gameService.RefuseInvitation(player);
                }
            };

            _eventHandler.NotifyPlayerConnectedEvent += player =>
            {
                Players.Add(player);
                NotifyOfPropertyChange(() => CanInviteRandom);
            };

            _eventHandler.NotifyPlayerDisconnectedEvent += player =>
            {
                var listedPlayer = Players.Single(p => p.Id == player.Id);
                Players.Remove(listedPlayer);
                NotifyOfPropertyChange(() => CanInviteRandom);
            };

            _eventHandler.GameStartedEvent += gameParams => GameCreated?.Invoke(gameParams, _me);

            _eventHandler.NotifyPlayerUpdatedEvent += player =>
            {
                if (Players.Remove(player))
                {
                    if (!player.InGame)
                    {
                        Players.Add(player);
                    }
                    NotifyOfPropertyChange(() => CanInviteRandom);
                }
            };
        }

        public PlayersViewModel(IGameService gameService, GameEventHandler eventHandler, Player player)
        {
            _gameService = gameService;
            _eventHandler = eventHandler;
            _me = player;

            Players = new ObservableCollection<Player>(_gameService.GetAvailablePlayers().Where(p => p.Id != _me.Id));
            RegisterEvents();

            GameTypes = Utils.Utils.GetGameTypes();
            SelectedGameType = GameTypes[0];
        }

        public bool CanInvitePlayer => SelectedPlayer != null && SelectedGameType != null;

        public bool CanInviteRandom => Players.Count > 0;

        public void InvitePlayer()
        {
            _gameService.InvitePlayer(SelectedPlayer, SelectedGameType.GameSize);
        }

        public void InviteRandom()
        {
            if (Players.Count <= 0) return;
            SelectedPlayer = Players[new Random().Next(0, Players.Count - 1)];
            InvitePlayer();
        }
    }
}
