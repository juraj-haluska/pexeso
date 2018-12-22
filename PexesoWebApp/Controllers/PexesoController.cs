using System;
using System.ServiceModel;
using System.Web.Mvc;
using GameService.Library;

namespace PexesoWebApp.Controllers
{
    public class PexesoController : Controller
    {
        private InstanceContext Ctx => new InstanceContext(new PexesoEventHandler());

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Player player)
        {
            var channelFactory = new DuplexChannelFactory<IGameService>(Ctx, "GameServiceEndPoint");
            var service = channelFactory.CreateChannel();

            try
            {
                var registered = service.RegisterPlayer(player.Name, player.Password);
                if (registered != null)
                {
                    return View("RegistrationSuccess");
                }

                return View("RegistrationError");
            }
            catch
            {
                return View("ServiceNotAvailable");
            }
        }

        public ActionResult Statistics()
        {
            var channelFactory = new DuplexChannelFactory<IGameService>(Ctx, "GameServiceEndPoint");
            var service = channelFactory.CreateChannel();
            try
            {   
                return View(service.GetGamesStatistics());
            }
            catch
            {
                return View("ServiceNotAvailable");
            }
        }
    }
}