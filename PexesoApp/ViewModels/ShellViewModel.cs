using System.ServiceModel;
using Caliburn.Micro;
using GameService.Library;

namespace PexesoApp.ViewModels
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, UseSynchronizationContext = false)]
    public class ShellViewModel : Conductor<object>, IGameClient
    {
        private readonly IGameService _gameService;
        private Player _player;

        public ShellViewModel()
        {
            // create wcf connection
            var ctx = new InstanceContext(this);
            var channelFactory = new DuplexChannelFactory<IGameService>(ctx, "GameServiceEndPoint");
            _gameService = channelFactory.CreateChannel();
        }

        protected override void OnActivate()
        {
            ShowStartScreen();
        }

        public void InvitedBy(Player player, GameParams gameParams)
        {
            throw new System.NotImplementedException();
        }

        public void InvitationAccepted(Player player)
        {
            throw new System.NotImplementedException();
        }

        public void InvitationRefused(Player player)
        {
            throw new System.NotImplementedException();
        }

        private void ShowStartScreen()
        {
            var startViewModel = new StartViewModel(_gameService)
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
                PlayerLoggedIn = player =>
                {
                    _player = player;
                    ShowPlayersScreen();
                }
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

        private void ShowPlayersScreen()
        {
            ActivateItem(new PlayersViewModel());
        }
    }
}
