using Microsoft.EntityFrameworkCore;

namespace InfiniteCaptcha.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<PlayerRecord> PlayerRecords { get; set; }
}