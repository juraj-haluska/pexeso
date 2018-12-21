using GameService.Library;

namespace GameService
{
    class Utils
    {
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
