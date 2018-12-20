using Caliburn.Micro;
using GameService.Library;

namespace PexesoApp.ViewModels
{
    public class StartViewModel : Screen
    {
        private readonly IGameService _gameService;

        public StartViewModel(IGameService gameService)
        {
            _gameService = gameService;
        }
    }
}