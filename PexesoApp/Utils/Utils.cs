using System.Collections.Generic;
using System.Collections.ObjectModel;
using GameService.Library;
using PexesoApp.ViewModels;

namespace PexesoApp.Utils
{
    public class Utils
    {
        public static ObservableCollection<GameTypeViewModel> GetGameTypes()
        {
            var items = new ObservableCollection<GameTypeViewModel>
            {
                new GameTypeViewModel {GameSize = GameParams.GameSizes.Size3X2, Name = "3 x 2"},
                new GameTypeViewModel {GameSize = GameParams.GameSizes.Size4X3, Name = "4 x 3"},
                new GameTypeViewModel {GameSize = GameParams.GameSizes.Size4X4, Name = "4 x 4"},
                new GameTypeViewModel {GameSize = GameParams.GameSizes.Size5X4, Name = "5 x 4"},
                new GameTypeViewModel {GameSize = GameParams.GameSizes.Size6X5, Name = "6 x 5"},
                new GameTypeViewModel {GameSize = GameParams.GameSizes.Size6X6, Name = "6 x 6"},
                new GameTypeViewModel {GameSize = GameParams.GameSizes.Size7X6, Name = "7 x 6"},
                new GameTypeViewModel {GameSize = GameParams.GameSizes.Size8X7, Name = "8 x 7"},
                new GameTypeViewModel {GameSize = GameParams.GameSizes.Size8X8, Name = "8 x 8"}
            };

            return items;
        }
    }
}
