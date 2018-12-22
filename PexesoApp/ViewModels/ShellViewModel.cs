using System.ServiceModel;
using System.Windows;
using Caliburn.Micro;
using GameService.Library;
using GameService.Library.Models;

namespace PexesoApp.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        private readonly GameEventHandler _eventHandler;
        private readonly IGameService _gameService;

        public ShellViewModel()
        {
            // create wcf connection
            _eventHandler = new GameEventHandler(Application.Current.Dispatcher);
            var ctx = new InstanceContext(_eventHandler);
            var channelFactory = new DuplexChannelFactory<IGameService>(ctx, "GameServiceEndPoint");
            _gameService = channelFactory.CreateChannel();
        }

        protected override void OnActivate()
        {
            ShowStartScreen();
        }

        private void ShowStartScreen()
        {
            var startViewModel = new StartViewModel
            {
                Login = ShowLoginScreen, Register = ShowRegisterScreen
            };
            ActivateItem(startViewModel);
        }

        private void ShowLoginScreen()
        {
            var loginViewModel = new LoginViewModel(_gameService)
            {
                ExitScreen = ShowStartScreen,
                PlayerLoggedIn = ShowPlayersScreen
            };
            ActivateItem(loginViewModel);
        }

        private void ShowRegisterScreen()
        {
            var registerViewModel = new RegisterViewModel(_gameService)
            {
                ExitScreen = ShowStartScreen
            };
            ActivateItem(registerViewModel);
        }

        private void ShowPlayersScreen(Player loggedPlayer)
        {
            var playersViewModel = new PlayersViewModel(_gameService, _eventHandler, loggedPlayer)
            {
                GameCreated = ShowGameScreen
            };
            ActivateItem(playersViewModel);
        }

        private void ShowGameScreen(GameParams gameParams, Player me)
        {
            var gameViewModel = new GameViewModel(gameParams, me, _gameService, _eventHandler)
            {
                Exit = () => TryClose(),
                GameFinished = ShowStatisticsScreen
            };
            ActivateItem(gameViewModel);
        }

        private void ShowStatisticsScreen()
        {
            ActivateItem(new StatisticsViewModel(_gameService));
        }
    }
}
