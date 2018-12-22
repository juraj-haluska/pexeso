using System;
using System.Windows;
using Caliburn.Micro;
using GameService.Library;
using Action = System.Action;

namespace PexesoApp.ViewModels
{
    public class LoginViewModel : Screen
    {
        private readonly IGameService _gameService;
        private string _playerName;
        private string _playerPassword;

        public Action ExitScreen { get; set; }

        public Action<Player> PlayerLoggedIn { get; set; }

        public LoginViewModel(IGameService gameService)
        {
            _gameService = gameService;
        }

        public string PlayerName
        {
            get => _playerName;
            set
            {
                _playerName = value;
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        public string PlayerPassword
        {
            get => _playerPassword;
            set
            {
                _playerPassword = value;
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        public bool CanLogin => PlayerName?.Length >= 5 && PlayerPassword?.Length >= 5;

        public void Login()
        {
            var player = _gameService.ConnectPlayer(PlayerName, PlayerPassword);

            if (player != null)
            {
                PlayerLoggedIn(player);
            }
            else
            {
                MessageBox.Show("Wrong name or password", "", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void Cancel()
        {
            ExitScreen?.Invoke();
        }
    }
}
