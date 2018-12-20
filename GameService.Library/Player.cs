using System.Runtime.Serialization;

namespace GameService.Library
{
    [DataContract]
    public class Player
    {
        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Password { get; set; }

        public Player(string name, string password)
        {
            Name = name;
            Password = password;
        }

        public Player()
        {
        }
    }
}
