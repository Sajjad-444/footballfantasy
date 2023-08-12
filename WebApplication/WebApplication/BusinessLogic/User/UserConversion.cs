namespace WebApplication.BusinessLogic;

using DataAccess;

public class UserConversion
{
    public static User convertTempUserToUser(TempUser tempUser)
    {
        if (tempUser == null)
        {
            return null;
        }

        return new User()
        {
            emailAddress = tempUser.emailAddress,
            firstName = tempUser.firstName,
            lastName = tempUser.lastName,
            OtpCode = tempUser.OtpCode,
            OtpCodeExpTime = tempUser.OtpCodeExpTime,
            password = tempUser.password,
            userName = tempUser.userName
        };
    }

    public static User convertdbUserToUser(DbUser dbUser)
    {
        if (dbUser == null)
        {
            return null;
        }

        return new User()
        {
            emailAddress = dbUser.emailAddress,
            firstName = dbUser.firstName,
            lastName = dbUser.lastName,
            password = dbUser.password,
            userName = dbUser.userName,
            cash = dbUser.cash,
            score = dbUser.score
        };
    }

    public static TempUser convertUsertoTempUser(User user)
    {
        if (user == null)
        {
            return null;
        }

        return new TempUser()
        {
            emailAddress = user.emailAddress,
            firstName = user.firstName,
            lastName = user.lastName,
            OtpCode = user.OtpCode,
            OtpCodeExpTime = user.OtpCodeExpTime,
            password = user.password,
            userName = user.userName
        };
    }

    public static DbUser convertUserToDbUser(User user)
    {
        if (user == null)
        {
            return null;
        }

        return new DbUser()
        {
            emailAddress = user.emailAddress,
            firstName = user.firstName,
            lastName = user.lastName,
            password = user.password,
            userName = user.userName,
            cash = user.cash,
            score = user.score
        };
    }

    public static List<User> convertDbUsersListToUsersList(List<DbUser> dbUsers)
    {
        List<User> users = new List<User>();
        foreach (var dbUser in dbUsers)
        {
            users.Add(convertdbUserToUser(dbUser));
        }

        return users;
    }
}