using System.ServiceModel;

namespace GameService.Library
{
    [ServiceContract (CallbackContract = typeof(IGameClient))]
    public interface IGameService
    {
        [OperationContract]
        Player PlayerRegister(string name, string password);

        [OperationContract]
        Player PlayerLogIn(string name, string password);

        [OperationContract]
        void PlayerConnect(Player player);

        [OperationContract]
        void Broadcast(string message);
    }
}
