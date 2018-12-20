using System.ServiceModel;
using Caliburn.Micro;
using GameService.Library;

namespace PexesoApp.ViewModels
{
    public class ShellViewModel : Conductor<object>, IGameClient
    {
        private readonly IGameService _gameService;
        private readonly Screen _startScreen;

        public ShellViewModel()
        {
            // create wcf connection
            var ctx = new InstanceContext(this);
            var channelFactory = new DuplexChannelFactory<IGameService>(ctx, "GameServiceEndPoint");
            _gameService = channelFactory.CreateChannel();

            // create screens
            _startScreen = new StartViewModel(_gameService);
        }

        protected override void OnActivate()
        {
            ActivateItem(_startScreen);
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
    }
}
