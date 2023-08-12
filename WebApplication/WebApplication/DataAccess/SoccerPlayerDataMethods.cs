using Microsoft.EntityFrameworkCore;
using WebApplication.BusinessLogic;

namespace WebApplication.DataAccess;

public class SoccerPlayerDataMethods
{
    public static void deleteSoccerPlayer(DbUser dbUser, DbSoccerPlayer dbSoccerPlayer)
    {
        using (var db = new Database())
        {
            var user = db.dbUsers.Include(u => u.userSoccerPlayers).SingleOrDefault(u => u.userName == dbUser.userName);
            if (user != null)
            {
                var userSoccerPlayer = user.userSoccerPlayers.SingleOrDefault(sp =>
                    sp.dbSoccerPlayerId == dbSoccerPlayer.dbSoccerPlayerId);
                if (userSoccerPlayer != null)
                {
                    user.userSoccerPlayers.Remove(userSoccerPlayer);
                    db.SaveChanges();
                }
            }
        }
    }

    public static void deleteAllDbSoccerPlayersFromTable()
    {
        using (var db = new Database())
        {
            var dbSoccerPlayers = db.dbSoccerPlayers.ToList();
            db.dbSoccerPlayers.RemoveRange(dbSoccerPlayers);
            db.SaveChanges();
        }
    }

    public static void addSoccerPlayerToDbUser(DbUser dbUser, DbSoccerPlayer dbSoccerPlayer, Role role)
    {
        using (var db = new Database())
        {
            var user = db.dbUsers.Include(u =>
                u.userSoccerPlayers).SingleOrDefault(u => u.userName == dbUser.userName);

            if (user != null)
            {
                UserSoccerPlayer userSoccerPlayer = new UserSoccerPlayer()
                {
                    dbUserId = user.userId,
                    dbSoccerPlayerId = dbSoccerPlayer.dbSoccerPlayerId,
                    role = role
                };
                user.userSoccerPlayers.Add(userSoccerPlayer);
                db.SaveChanges();
            }
        }
    }

    public static bool soccerPlayerExistence(DbUser dbUser, int dbSoccerPlayerId, Role role)
    {
        using (var db = new Database())
        {
            var user = db.dbUsers.Include(u => u.userSoccerPlayers).SingleOrDefault(u => u.userName == dbUser.userName);

            if (user != null)
            {
                return user.userSoccerPlayers.Any(usp => usp.dbSoccerPlayerId == dbSoccerPlayerId && usp.role == role);
            }

            return false;
        }
    }

    public static int countUserSoccerPlayers(DbUser dbUser)
    {
        using (var db = new Database())
        {
            var user = db.dbUsers.Include(u =>
                u.userSoccerPlayers).SingleOrDefault(u => u.userName == dbUser.userName);
            return user.userSoccerPlayers.Count;
        }
    }

    public static int countUserSoccerPlayersByRole(DbUser dbUser, Role role)
    {
        int counter = 0;
        using (var db = new Database())
        {
            var user = db.dbUsers.Include(u =>
                u.userSoccerPlayers).SingleOrDefault(u => u.userName == dbUser.userName);
            foreach (var userSoccerPlayer in user.userSoccerPlayers)
            {
                if (userSoccerPlayer.role == role)
                {
                    counter++;
                }
            }
        }

        return counter;
    }

    public static int countUserSoccerPlayersByPosition(DbUser dbUser, Position position)
    {
        using (var db = new Database())
        {
            var user = db.dbUsers.Include(u =>
                u.userSoccerPlayers).SingleOrDefault(u => u.userName == dbUser.userName);
            int counter = 0;
            foreach (var userSoccerPlayer in user.userSoccerPlayers)
            {
                var soccerPlayer = db.dbSoccerPlayers.Find(userSoccerPlayer.dbSoccerPlayerId);
                if (soccerPlayer != null && soccerPlayer.position == position)
                {
                    counter++;
                }
            }

            return counter;
        }
    }

    public static int countUserSoccerPlayersByTeam(DbUser dbUser, string team)
    {
        using (var db = new Database())
        {
            var user = db.dbUsers.Include(u =>
                u.userSoccerPlayers).SingleOrDefault(u => u.userName == dbUser.userName);
            int counter = 0;
            foreach (var userSoccerPlayer in user.userSoccerPlayers)
            {
                var soccerPlayer = db.dbSoccerPlayers.Find(userSoccerPlayer.dbSoccerPlayerId);
                if (soccerPlayer != null && soccerPlayer.team == team)
                {
                    counter++;
                }
            }

            return counter;
        }
    }

    public static void addSoccerPlayerToDbSoccerPlayerTable(DbSoccerPlayer dbSoccerPlayer)
    {
        using (var db = new Database())
        {
            db.dbSoccerPlayers.Add(dbSoccerPlayer);
            db.SaveChanges();
        }
    }

    public static void swapSoccerPlayersRole(DbUser dbUser, int dbSoccerPlayerId1, int dbSoccerPlayerId2)
    {
        using (var db = new Database())
        {
            var user = db.dbUsers.Include(u => u.userSoccerPlayers).SingleOrDefault(u => u.userName == dbUser.userName);
            if (user != null)
            {
                var userSoccerPlayer1 =
                    user.userSoccerPlayers.FirstOrDefault(usp => usp.dbSoccerPlayerId == dbSoccerPlayerId1);
                var userSoccerPlayer2 =
                    user.userSoccerPlayers.FirstOrDefault(usp => usp.dbSoccerPlayerId == dbSoccerPlayerId2);

                if (userSoccerPlayer1 != null && userSoccerPlayer2 != null)
                {
                    Role tempRole = userSoccerPlayer1.role;
                    userSoccerPlayer1.role = userSoccerPlayer2.role;
                    userSoccerPlayer2.role = tempRole;
                    db.SaveChanges();
                }
            }
        }
    }

    public static List<DbSoccerPlayer> filterByName(string name)
    {
        List<DbSoccerPlayer> result = new List<DbSoccerPlayer>();
        using (var db = new Database())
        {
            result = db.dbSoccerPlayers.Where(dsp =>
                    dsp.firstName.ToLower().Contains(name) || dsp.lastName.ToLower().Contains(name))
                .ToList();
        }

        return result;
    }

    public static List<DbSoccerPlayer> filterByScore(int scoreStart, int scoreEnd)
    {
        List<DbSoccerPlayer> result = new List<DbSoccerPlayer>();
        using (var db = new Database())
        {
            result = db.dbSoccerPlayers.Where(dsp => dsp.score >= scoreStart && dsp.score <= scoreEnd).ToList();
        }

        return result;
    }

    public static List<DbSoccerPlayer> filterByPrice(double priceStart, double priceEnd)
    {
        List<DbSoccerPlayer> result = new List<DbSoccerPlayer>();
        using (var db = new Database())
        {
            result = db.dbSoccerPlayers.Where(dsp => dsp.price >= priceStart && dsp.price <= priceEnd).ToList();
        }

        return result;
    }

    public static List<DbSoccerPlayer> filterByPosition(Position position)
    {
        List<DbSoccerPlayer> result = new List<DbSoccerPlayer>();
        using (var db = new Database())
        {
            result = db.dbSoccerPlayers.Where(dsp => dsp.position == position).ToList();
        }

        return result;
    }

    public static List<DbSoccerPlayer> getAllDbSoccerPlayers()
    {
        using (var db = new Database())
        {
            return db.dbSoccerPlayers.ToList();
        }
    }

    public static List<Pair<DbSoccerPlayer, Role>> getUserSoccerPlayers(DbUser dbUser)
    {
        using (var db = new Database())
        {
            var user = db.dbUsers.Include(u =>
                u.userSoccerPlayers).SingleOrDefault(u => u.userName == dbUser.userName);
            List<Pair<DbSoccerPlayer, Role>> userDbSoccerPlayers = new List<Pair<DbSoccerPlayer, Role>>();

            if (user != null)
            {
                foreach (var userSoccerPlayer in user.userSoccerPlayers)
                {
                    Pair<DbSoccerPlayer, Role> userDbSoccerPlayer = new Pair<DbSoccerPlayer, Role>();
                    userDbSoccerPlayer.first = db.dbSoccerPlayers.Find(userSoccerPlayer.dbSoccerPlayerId);
                    userDbSoccerPlayer.second = userSoccerPlayer.role;
                    userDbSoccerPlayers.Add(userDbSoccerPlayer);
                }
            }

            return userDbSoccerPlayers;
        }
    }
}