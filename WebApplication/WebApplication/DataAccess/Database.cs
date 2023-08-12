using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using WebApplication.BusinessLogic;

namespace WebApplication.DataAccess;

public class Database : DbContext
{
    public DbSet<DbUser> dbUsers { get; set; }
    public DbSet<TempUser> tempUsers { get; set; }

    public DbSet<DbSoccerPlayer> dbSoccerPlayers { get; set; }
    public DbSet<UserSoccerPlayer> userSoccerPlayers { get; set; }
    public DbSet<DbUserEmailConfirmation> DbUserEmailConfirmations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserSoccerPlayer>()
            .HasKey(usp => new { usp.dbUserId, usp.dbSoccerPlayerId });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder contextOptionsBuilder)
    {
        contextOptionsBuilder.UseSqlite("Data source=database.db");
    }
}

public class DbUser
{
    [Key] public int userId { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string emailAddress { get; set; }
    public string userName { get; set; }
    public string password { get; set; }
    public int score { get; set; }
    public double cash { get; set; }
    public ICollection<UserSoccerPlayer> userSoccerPlayers { get; set; }
}

public class TempUser
{
    [Key] public int id { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string emailAddress { get; set; }
    public string userName { get; set; }
    public string password { get; set; }
    public string OtpCode { get; set; }
    public DateTime OtpCodeExpTime { get; set; }
}

public class DbSoccerPlayer
{
    [Key] public int dbSoccerPlayerId { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public Position position { get; set; }
    public int score { get; set; }
    public string team { get; set; }
    public double price { get; set; }
    public string photo { get; set; }
}

public class UserSoccerPlayer
{
    public int dbUserId { get; set; }
    public int dbSoccerPlayerId { get; set; }
    public Role role { get; set; }
}

public class DbUserEmailConfirmation
{
    [Key] public string emailAddress { get; set; }
    public string OtpCode { get; set; }
}