using System.ServiceModel;
using System.Windows.Threading;
using GameService.Library;

namespace PexesoApp
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, UseSynchronizationContext = false)]
    public class GameEventHandler : IGameClient
    {
        private readonly Dispatcher _dispatcher;

        public GameEventHandler(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public delegate void InvitedByDel(Player player, GameParams.GameSizes gameSize);
        public delegate void GameStartedDel(GameParams gameParams);
        public delegate void InvitationRefusedDel(Player player);
        public delegate void NotifyPlayerConnectedDel(Player player);
        public delegate void NotifyPlayerDisconnectedDel(Player player);
        public delegate void NotifyPlayerUpdatedDel(Player player);
        public delegate void RevealCardDel(int cardIndex, int cardValue);
        public delegate void SwapPlayerTurnDel(int card1Index, int card2Index, int card1Value, int card2Value);
        public delegate void CardPairFoundDel(int card1Index, int card2Index, int card2Value);

        public event InvitedByDel InvitedByEvent;
        public event GameStartedDel GameStartedEvent;
        public event InvitationRefusedDel InvitationRefusedEvent;
        public event NotifyPlayerConnectedDel NotifyPlayerConnectedEvent;
        public event NotifyPlayerDisconnectedDel NotifyPlayerDisconnectedEvent;
        public event NotifyPlayerUpdatedDel NotifyPlayerUpdatedEvent;
        public event RevealCardDel RevealCardEvent;
        public event SwapPlayerTurnDel SwapPlayerTurnEvent;
        public event CardPairFoundDel CardPairFoundEvent;

        public async void InvitedBy(Player player, GameParams.GameSizes gameSize)
        {
            if (InvitedByEvent == null) return;
            var par = new object[] { player, gameSize };
            await _dispatcher.BeginInvoke(InvitedByEvent, par);
        }

        public async void StartGame(GameParams gameParams)
        {
            if (GameStartedEvent == null) return;
            var par = new object[] { gameParams };
            await _dispatcher.BeginInvoke(GameStartedEvent, par);
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

        public async void RevealCard(int cardIndex, int cardValue)
        {
            if (RevealCardEvent == null) return;
            var par = new object[] { cardIndex, cardValue };
            await _dispatcher.BeginInvoke(RevealCardEvent, par);
        }

        public async void SwapPlayerTurn(int card1Index, int card2Index, int card1Value, int card2Value)
        {
            if (SwapPlayerTurnEvent == null) return;
            var par = new object[] { card1Index, card2Index, card1Value, card2Value};
            await _dispatcher.BeginInvoke(SwapPlayerTurnEvent, par);
        }

        public async void CardPairFound(int card1Index, int card2Index, int card2Value)
        {
            if (CardPairFoundEvent == null) return;
            var par = new object[] { card1Index, card2Index, card2Value };
            await _dispatcher.BeginInvoke(CardPairFoundEvent, par);
        }
    }
}
