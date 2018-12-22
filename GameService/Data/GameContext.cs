using System.Data.Entity;
using GameService.Library;

namespace GameService.Data
{
    public class GameContext : DbContext
    {
        public GameContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<Player> Players { get; set; }

        public DbSet<GameStatistic> Games { get; set; }
    }
}
