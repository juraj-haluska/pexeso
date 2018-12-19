using System.ServiceModel;

namespace GameService.Library
{
    public interface IGameClient
    {
        [OperationContract(IsOneWay = true)]
         void TestMethod(string message);
    }
}