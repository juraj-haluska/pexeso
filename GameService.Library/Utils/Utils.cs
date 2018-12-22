namespace GameService.Library.Utils
{
    public static class Utils
    {
        public static string GetGameTypeName(GameParams.GameSizes gameSize)
        {
            switch (gameSize)
            {
                case GameParams.GameSizes.Size3X2:
                    return "3 x 2";
                case GameParams.GameSizes.Size4X3:
                    return "4 x 3";
                case GameParams.GameSizes.Size4X4:
                    return "4 x 4";
                case GameParams.GameSizes.Size5X4:
                    return "5 x 4";
                case GameParams.GameSizes.Size6X5:
                    return "6 x 5";
                case GameParams.GameSizes.Size6X6:
                    return "6 x 6";
                case GameParams.GameSizes.Size7X6:
                    return "7 x 6";
                case GameParams.GameSizes.Size8X7:
                    return "8 x 7";
                // case GameParams.GameSizes.Size8X8:
                default:
                    return "8 x 8";
            }
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
                    return;
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
                // case GameParams.GameSizes.Size8X8:
                default:
                    rows = 8;
                    cols = 8;
                    return;
            }
        }

        public static string GetGameResult(GameResult result)
        {
            switch (result)
            {
                case GameResult.Win:
                    return "winner";
                case GameResult.Lose:
                    return "looser";
                // case GameResult.FiftyFifty:  
                default:
                    return "as good as your opponent";
            }
        }
    }
}
