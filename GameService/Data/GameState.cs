using GameService.Library;

namespace GameService.Data
{
    class GameState
    {
        public Player FirstPlayer { get; set; }

        public Player SecondPlayer { get; set; }

        public int[] Map { get; set; }

        public int? RevealedCardIndex { get; set; }

        private int FirstPlayerScore { get; set; }

        private int SecondPlayerScore { get; set; }

        public void IncrementScore(Player player)
        {
            if (player.Equals(FirstPlayer))
            {
                FirstPlayerScore++;
            }
            else if (player.Equals(SecondPlayer))
            {
                SecondPlayerScore++;
            }
        }

        public bool IsGameEnd()
        {
            return (FirstPlayerScore + SecondPlayerScore) * 2 == Map.Length;
        }
    }
}
