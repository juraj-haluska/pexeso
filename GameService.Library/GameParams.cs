using System;
using System.Runtime.Serialization;

namespace GameService.Library
{
    [DataContract]
    public class GameParams
    {
        public enum GameSizes
        {
            Size3X2,
            Size4X3,
            Size4X4,
            Size5X4,
            Size6X5,
            Size6X6,
            Size7X6,
            Size8X7,
            Size8X8
        }

        [DataMember]
        public Player FirstPlayer { get; set; }

        [DataMember]
        public Player SecondPlayer { get; set; }

        [DataMember]
        public GameSizes GameSize { get; set; }

        public GameParams()
        {
        }

        public GameParams(Player firstPlayer, Player secondPlayer, GameSizes gameSize)
        {
            FirstPlayer = firstPlayer;
            SecondPlayer = secondPlayer;
            GameSize = gameSize;
        }

        public void RandomizeStartingPlayer()
        {
            if (new Random(DateTime.Now.Millisecond).Next() % 2 == 0)
            {
                var tempPlayer = FirstPlayer;
                FirstPlayer = SecondPlayer;
                SecondPlayer = tempPlayer;
            }
            else
            {
                var tempPlayer = SecondPlayer;
                SecondPlayer = FirstPlayer;
                FirstPlayer = tempPlayer;
            }
        }
    }
}
