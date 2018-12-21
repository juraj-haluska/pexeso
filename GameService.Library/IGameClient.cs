using System.ServiceModel;

namespace GameService.Library
{
    public interface IGameClient
    {
        [OperationContract(IsOneWay = true)]
        void InvitedBy(Player player, GameParams.GameSizes gameSize);

        [OperationContract(IsOneWay = true)]
        void StartGame(GameParams gameParams);

        [OperationContract(IsOneWay = true)]
        void InvitationRefused(Player player);

        [OperationContract(IsOneWay = true)]
        void NotifyPlayerConnected(Player player);

        [OperationContract(IsOneWay = true)]
        void NotifyPlayerDisconnected(Player player);

        [OperationContract(IsOneWay = true)]
        void NotifyPlayerUpdate(Player player);

        [OperationContract(IsOneWay = true)]
        void RevealCard(int cardIndex, int cardValue);

        [OperationContract(IsOneWay = true)]
        void SwapPlayerTurn(int card1Index, int card2Index, int card1Value, int card2Value);

        [OperationContract(IsOneWay = true)]
        void CardPairFound(int card1Index, int card2Index, int card2Value);

        [OperationContract(IsOneWay = true)]
        void IncomingMessage(Player from, string message);

        [OperationContract(IsOneWay = true)]
        void GameTimeout();

        [OperationContract(IsOneWay = true)]
        void GameFinished(GameResult result);
    }
}