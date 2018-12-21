using System.Collections.Generic;
using System.ServiceModel;

namespace GameService.Library
{
    [ServiceContract (CallbackContract = typeof(IGameClient))]
    public interface IGameService
    {
        [OperationContract]
        Player RegisterPlayer(string name, string password);

        [OperationContract]
        Player ConnectPlayer(string name, string password);

        [OperationContract]
        List<Player> GetAvailablePlayers();

        [OperationContract]
        void InvitePlayer(Player player, GameParams.GameSizes gameSize);

        [OperationContract]
        void AcceptInvitation(Player player, GameParams.GameSizes gameSize);

        [OperationContract]
        void RefuseInvitation(Player player);

        [OperationContract]
        void DisconnectPlayer(Player player);

        [OperationContract]
        void RevealCardRequest(Player player, int cardIndex);

        [OperationContract]
        void SendMessage(string message);

        [OperationContract]
        void GameTimeout();

        [OperationContract]
        List<GameState> GetGamesStatistics();
    }
}
