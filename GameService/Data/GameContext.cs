using System.Data.Entity;
using GameService.Library;

namespace GameService.Data
{
    public class GameContext : DbContext
    {
        public DbSet<Player> Players { get; set; }

        public DbSet<GameState> Games { get; set; }
    }
}
