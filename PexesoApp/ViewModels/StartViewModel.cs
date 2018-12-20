using Caliburn.Micro;
using GameService.Library;
using Action = System.Action;

namespace PexesoApp.ViewModels
{
    public class StartViewModel : Screen
    {
        private readonly IGameService _gameService;

        public Action Register { get; set; }

        public Action Login { get; set; }

        public StartViewModel(IGameService gameService)
        {
            _gameService = gameService;
        }

        public void OnRegister()
        {
            Register();
        }

        public void OnLogin()
        {
            Login();
        }
    }
}