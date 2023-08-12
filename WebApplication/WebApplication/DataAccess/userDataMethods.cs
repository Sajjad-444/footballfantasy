using Microsoft.EntityFrameworkCore;

namespace WebApplication.DataAccess;

using BusinessLogic;

public class userDataMethods
{
    public static bool userNameInMain(string userName)
    {
        using (var db = new Database())
        {
            var dbUser = db.dbUsers.FirstOrDefault(dUser => dUser.userName == userName);
            if (dbUser != null)
            {
                return true;
            }

            return false;
        }
    }

    public static bool emailInMain(string emailAddress)
    {
        using (var db = new Database())
        {
            var dbUser = db.dbUsers.FirstOrDefault(dUser => dUser.emailAddress == emailAddress);
            if (dbUser != null)
            {
                return true;
            }

            return false;
        }
    }

    public static bool userNameInTemp(string userName)
    {
        using (var db = new Database())
        {
            var tempUser = db.tempUsers.FirstOrDefault(dUser => dUser.userName == userName);
            if (tempUser != null)
            {
                return true;
            }

            return false;
        }
    }

    public static bool emailInTemp(string emailAddress)
    {
        using (var db = new Database())
        {
            var tempUser = db.tempUsers.FirstOrDefault(dUser => dUser.emailAddress == emailAddress);
            if (tempUser != null)
            {
                return true;
            }

            return false;
        }
    }

    public static DbUser findDbUserByUserName(string userName)
    {
        using (var db = new Database())
        {
            var dbUser = db.dbUsers.FirstOrDefault(dUser => dUser.userName == userName);
            return dbUser;
        }
    }

    public static DbUser findDbUserByEmail(string email)
    {
        using (var db = new Database())
        {
            var dbbUser = db.dbUsers.FirstOrDefault(dUser => dUser.emailAddress == email);
            return dbbUser;
        }
    }

    public static TempUser findTempUser(string userName)
    {
        using (var db = new Database())
        {
            var tempUser = db.tempUsers.FirstOrDefault(tUser => tUser.userName == userName);
            return tempUser;
        }
    }

    public static void addUserToMainTable(DbUser user)
    {
        using (var db = new Database())
        {
            db.dbUsers.Add(user);
            db.SaveChanges();
        }
    }

    public static void addUserToTmepUsersTable(TempUser user)
    {
        using (var db = new Database())
        {
            db.tempUsers.Add(user);
            db.SaveChanges();
        }
    }

    public static void increaseDbUserCash(DbUser dbUser, double price)
    {
        using (var db = new Database())
        {
            var user = db.dbUsers.FirstOrDefault(user => user.userName == dbUser.userName);
            if (user != null)
            {
                user.cash += price;
                db.dbUsers.Update(user);
                db.SaveChanges();
            }
        }
    }

    public static void updateTempUserOtp(TempUser tempUser)
    {
        using (var db = new Database())
        {
            var temp = db.tempUsers.FirstOrDefault(temp => temp.userName == tempUser.userName);
            if (temp != null)
            {
                temp.OtpCode = tempUser.OtpCode;
                temp.OtpCodeExpTime = tempUser.OtpCodeExpTime;
                db.tempUsers.Update(temp);
                db.SaveChanges();
            }
        }
    }

    public static void deleteTempUser(TempUser tempUser)
    {
        using (var db = new Database())
        {
            var temp = db.tempUsers.FirstOrDefault(temp => temp.userName == tempUser.userName);
            db.tempUsers.Remove(temp);
            db.SaveChanges();
        }
    }

    public static void deleteExpiredTempUsers()
    {
        using (var db = new Database())
        {
            var expiredTempUsers = db.tempUsers
                .Where(tempUser => DateTime.UtcNow > tempUser.OtpCodeExpTime.AddDays(7)).ToList();
            if (expiredTempUsers.Any())
            {
                db.tempUsers.RemoveRange(expiredTempUsers);
                db.SaveChanges();
            }
        }
    }

    public static void updateUserScore(DbUser dbUser, int score)
    {
        using (var db = new Database())
        {
            var user = db.dbUsers.SingleOrDefault(u => u.userName == dbUser.userName);
            if (user != null)
            {
                user.score = score;
                db.dbUsers.Update(user);
                db.SaveChanges();
            }
        }
    }

    public static List<DbUser> getAllDbUsers()
    {
        using (var db = new Database())
        {
            return db.dbUsers.ToList();
        }
    }

    public static void removeDbUser(DbUser dbUser)
    {
        using (var db = new Database())
        {
            var user = db.dbUsers.SingleOrDefault(u => u.userName == dbUser.userName);
            if (user != null)
            {
                db.dbUsers.Remove(user);
                db.SaveChanges();
            }
        }
    }

    public static void deleteUserSoccerPlayers(DbUser dbUser)
    {
        using (var db = new Database())
        {
            var user = db.dbUsers.Include(u => u.userSoccerPlayers)
                .SingleOrDefault(u => u.userName == dbUser.userName);
            if (user != null)
            {
                user.userSoccerPlayers.Clear();
                db.dbUsers.Update(user);
                db.SaveChanges();
            }
        }
    }

    public static void resetDbUserCash(DbUser dbUser)
    {
        using (var db = new Database())
        {
            var user = db.dbUsers.FirstOrDefault(user => user.userName == dbUser.userName);
            if (user != null)
            {
                user.cash = 100;
                db.dbUsers.Update(user);
                db.SaveChanges();
            }
        }
    }

    public static void addItemToDbUserEmailConfirmation(string email, string OtpCode)
    {
        DbUserEmailConfirmation item = new DbUserEmailConfirmation();
        item.emailAddress = email;
        item.OtpCode = OtpCode;
        using (var db = new Database())
        {
            db.DbUserEmailConfirmations.Add(item);
            db.SaveChanges();
        }
    }

    public static void deleteDbUserEmailConfirmation(string email)
    {
        using (var db = new Database())
        {
            DbUserEmailConfirmation temp =
                db.DbUserEmailConfirmations.FirstOrDefault(duec => duec.emailAddress == email);
            if (temp != null)
            {
                db.DbUserEmailConfirmations.Remove(temp);
                db.SaveChanges();
            }
        }
    }

    public static string getDbUserEmailConfirmationOtpCode(string email)
    {
        string otpCode;
        using (var db = new Database())
        {
            DbUserEmailConfirmation temp =
                db.DbUserEmailConfirmations.FirstOrDefault(duec => duec.emailAddress == email);
            if (temp == null)
            {
                return null;
            }

            return temp.OtpCode;
        }
    }

    public static void updateDbUserPassword(string email, string password)
    {
        using (var db = new Database())
        {
            DbUser dbUser = db.dbUsers.FirstOrDefault(du => du.emailAddress == email);
            if (dbUser != null)
            {
                dbUser.password = password;
                db.dbUsers.Update(dbUser);
                db.SaveChanges();
            }
        }
    }

    public static void purgeDbUserEmailConfirmations()
    {
        using (var db = new Database())
        {
            db.DbUserEmailConfirmations.RemoveRange(db.DbUserEmailConfirmations.ToList());
        }
    }
}