using System.Collections.Generic;
using Caliburn.Micro;
using GameService.Library;
using GameService.Library.Models;

namespace PexesoApp.ViewModels
{
    public class StatisticsViewModel : Screen
    {
        public List<GameStatistic> Statistics { get; }

        public StatisticsViewModel(IGameService gameService)
        {
            Statistics = gameService.GetGamesStatistics();
        }
    }
}
