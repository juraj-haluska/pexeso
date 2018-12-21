using Caliburn.Micro;
using GameService.Library;

namespace PexesoApp.ViewModels
{
    public class GameViewModel : Screen
    {
        private readonly GameParams _gameParams;
        private readonly Player _me;

        public bool StartingPlayer { get; set; }

        public GameViewModel(GameParams gameParams, Player me)
        {
            _gameParams = gameParams;
            _me = me;

            StartingPlayer = gameParams.FirstPlayer.Equals(me);
        }
    }
}
