using Microsoft.EntityFrameworkCore;

namespace MossadAgentsRest.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            //Seed();
        }


    }
}
