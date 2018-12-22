using System.ComponentModel.DataAnnotations.Schema;
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

        public string Password { get; set; }

        [DataMember]
        [NotMapped]
        public bool InGame { get; set; }

        public Player(string name, string password)
        {
            Name = name;
            Password = password;
        }

        public Player()
        {
        }

        protected bool Equals(Player other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Player) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Password != null ? Password.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
