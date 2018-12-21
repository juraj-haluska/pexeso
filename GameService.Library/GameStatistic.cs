using System.Runtime.Serialization;

namespace GameService.Library
{
    [DataContract]
    public class GameStatistic
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string FirstPlayer { get; set; }

        [DataMember]
        public string SecondPlayer { get; set; }

        [DataMember]
        public int FirstPlayerScore { get; set; }

        [DataMember]
        public int SecondPlayerScore { get; set; }

        [DataMember]
        public string GameSize { get; set; }

        [DataMember]
        public int GameDuration { get; set; }
    }
}
