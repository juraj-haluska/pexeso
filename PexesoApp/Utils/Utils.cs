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

        public static void GetGameSize(GameParams.GameSizes gameSize, out int rows, out int cols)
        {
            switch (gameSize)
            {
                case GameParams.GameSizes.Size3X2:
                    rows = 3;
                    cols = 2;
                    return;
                case GameParams.GameSizes.Size4X3:
                    rows = 4;
                    cols = 3;
                    return;
                case GameParams.GameSizes.Size4X4:
                    rows = 4;
                    cols = 4;
                    return; ;
                case GameParams.GameSizes.Size5X4:
                    rows = 5;
                    cols = 4;
                    return;
                case GameParams.GameSizes.Size6X5:
                    rows = 6;
                    cols = 5;
                    return;
                case GameParams.GameSizes.Size6X6:
                    rows = 6;
                    cols = 6;
                    return;
                case GameParams.GameSizes.Size7X6:
                    rows = 7;
                    cols = 6;
                    return;
                case GameParams.GameSizes.Size8X7:
                    rows = 8;
                    cols = 7;
                    return;
                case GameParams.GameSizes.Size8X8:
                default:
                    rows = 8;
                    cols = 8;
                    return;
            }
        }
    }
}
