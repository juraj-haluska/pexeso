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
        public GameSizes GameSize { get; set; }
    }
}