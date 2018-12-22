using System.Windows;
using Caliburn.Micro;
using GameService.Library;
using Action = System.Action;

namespace PexesoApp.ViewModels
{
    public class RegisterViewModel : Screen
    {
        public const int MinNameLength = 5;
        public const int MinPassLength = 5;

        private readonly IGameService _gameService;
        private string _playerName;
        private string _playerPassword;

        public Action ExitScreen { get; set; }

        public RegisterViewModel(IGameService gameService)
        {
            _gameService = gameService;
        }

        public string PlayerName
        {
            get => _playerName;
            set
            {
                _playerName = value;
                NotifyOfPropertyChange(() => CanRegister);
            } 
        }

        public string PlayerPassword
        {
            get => _playerPassword;
            set
            {
                _playerPassword = value;
                NotifyOfPropertyChange(() => CanRegister);
            }
        }

        public bool CanRegister => PlayerName?.Length >= MinNameLength && PlayerPassword?.Length >= MinPassLength;
 
    
        public void Register()
        {
            var registeredPlayer = _gameService.RegisterPlayer(PlayerName, PlayerPassword);

            if (registeredPlayer != null)
            {
                MessageBox.Show($"Player {registeredPlayer.Name} has been registered", "", MessageBoxButton.OK, 
                    MessageBoxImage.Information);

                ExitScreen?.Invoke();
            }
            else
            {
                MessageBox.Show($"Name {PlayerName} is already occupied.", "", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void Cancel()
        {
            ExitScreen?.Invoke();
        }
    }
}