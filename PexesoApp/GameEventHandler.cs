using System.ServiceModel;
using System.Windows.Threading;
using GameService.Library;

namespace PexesoApp
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, UseSynchronizationContext = false)]    
    class GameEventHandler : IGameClient
    {
        private readonly Dispatcher _dispatcher;

        public GameEventHandler(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public delegate void InvitedByDel(Player player, GameParams gameParams);
        public delegate void InvitationAcceptedDel(Player player);
        public delegate void InvitationRefusedDel(Player player);
        public delegate void NotifyPlayerConnectedDel(Player player);
        public delegate void NotifyPlayerDisconnectedDel(Player player);
        public delegate void NotifyPlayerUpdatedDel(Player player);

        public event InvitedByDel InvitedByEvent;
        public event InvitationAcceptedDel InvitationAcceptedEvent;
        public event InvitationRefusedDel InvitationRefusedEvent;
        public event NotifyPlayerConnectedDel NotifyPlayerConnectedEvent;
        public event NotifyPlayerDisconnectedDel NotifyPlayerDisconnectedEvent;
        public event NotifyPlayerUpdatedDel NotifyPlayerUpdatedEvent;

        public async void InvitedBy(Player player, GameParams gameParams)
        {
            if (InvitedByEvent == null) return;
            var par = new object[] { player, gameParams };
            await _dispatcher.BeginInvoke(InvitedByEvent, par);
        }

        public async void InvitationAccepted(Player player)
        {
            if (InvitationAcceptedEvent == null) return;
            var par = new object[] { player };
            await _dispatcher.BeginInvoke(InvitationAcceptedEvent, par);
        }

        public async void InvitationRefused(Player player)
        {
            if (InvitationRefusedEvent == null) return;
            var par = new object[] { player };
            await _dispatcher.BeginInvoke(InvitationRefusedEvent, par);
        }

        public async void NotifyPlayerConnected(Player player)
        {
            if (NotifyPlayerConnectedEvent == null) return;
            var par = new object[] { player };
            await _dispatcher.BeginInvoke(NotifyPlayerConnectedEvent, par);
        }

        public async void NotifyPlayerDisconnected(Player player)
        {
            if (NotifyPlayerDisconnectedEvent == null) return;
            var par = new object[] { player };
            await _dispatcher.BeginInvoke(NotifyPlayerDisconnectedEvent, par);
        }

        public async void NotifyPlayerUpdate(Player player)
        {
            if (NotifyPlayerUpdatedEvent == null) return;
            var par = new object[] { player };
            await _dispatcher.BeginInvoke(NotifyPlayerUpdatedEvent, par);
        }
    }
}
