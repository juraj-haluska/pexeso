using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameService.Library;

namespace TestApp
{
    class ClientCode : IGameClient
    {
        public void TestMethod(string message)
        {
            Console.WriteLine(message);
        }
    }
}
