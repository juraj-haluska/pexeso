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
    }
}