using Microsoft.EntityFrameworkCore;

namespace PexesoService.Data
{
    public class PexesoContext : DbContext
    {
        public PexesoContext(DbContextOptions options) : base(options)
        {
        }
    }
}
