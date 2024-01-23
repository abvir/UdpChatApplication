using Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class ChatContext : DbContext
{
    public DbSet<Message> Messages { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Db\\ChatDb.db");
    }
}