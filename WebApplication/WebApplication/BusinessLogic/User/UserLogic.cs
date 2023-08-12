using WebApplication.Presentation;

namespace WebApplication.BusinessLogic;

using DataAccess;

public class UserLogic
{
    public static UsernameEmailExistence UserExistence(string userName, string emailAddress)
    {
        if (userDataMethods.userNameInMain(userName) && userDataMethods.emailInMain(emailAddress))
        {
            return UsernameEmailExistence.UsernameAndEmailInMain;
        }

        if (userDataMethods.userNameInMain(userName))
        {
            return UsernameEmailExistence.UsernameInMain;
        }

        if (userDataMethods.emailInMain(emailAddress))
        {
            return UsernameEmailExistence.EmailInMain;
        }

        if (userDataMethods.userNameInTemp(userName) && userDataMethods.emailInTemp(emailAddress))
        {
            return UsernameEmailExistence.UsernameAndEmailInTemp;
        }

        if (userDataMethods.userNameInTemp(userName))
        {
            return UsernameEmailExistence.UsernameInTemp;
        }

        if (userDataMethods.emailInTemp(emailAddress))
        {
            return UsernameEmailExistence.EmailInTemp;
        }

        return UsernameEmailExistence.NotExistence;
    }

    public static void addUserToMainTable(User user)
    {
        user.cash = 100;
        userDataMethods.addUserToMainTable(UserConversion.convertUserToDbUser(user));
    }

    public static void addUserToTempTable(User user)
    {
        userDataMethods.addUserToTmepUsersTable(UserConversion.convertUsertoTempUser(user));
    }

    public static void increaseUserCash(User user, double price)
    {
        userDataMethods.increaseDbUserCash(UserConversion.convertUserToDbUser(user), price);
    }

    public static void updateTempUserOtp(User user)
    {
        userDataMethods.updateTempUserOtp(UserConversion.convertUsertoTempUser(user));
    }

    public static void deleteTempUser(User user)
    {
        userDataMethods.deleteTempUser(UserConversion.convertUsertoTempUser(user));
    }

    public static void deleteExpiredTempUsers()
    {
        userDataMethods.deleteExpiredTempUsers();
    }

    public static User findTempUser(string userName)
    {
        return UserConversion.convertTempUserToUser(userDataMethods.findTempUser(userName));
    }

    public static User findMainUserByUserName(string userName)
    {
        return UserConversion.convertdbUserToUser(userDataMethods.findDbUserByUserName(userName));
    }

    public static User findMainUserByEmail(string email)
    {
        return UserConversion.convertdbUserToUser(userDataMethods.findDbUserByEmail(email));
    }
    private static int calculateUserScore(User user)
    {
        int score = 0;
        List<Pair<DbSoccerPlayer, Role>> userDbSoccerPlayers =
            SoccerPlayerDataMethods.getUserSoccerPlayers(UserConversion.convertUserToDbUser(user));
        foreach (var userDbSoccerPlayer in userDbSoccerPlayers)
        {
            if (userDbSoccerPlayer.second == Role.MAIN)
            {
                score += 2 * userDbSoccerPlayer.first.score;
            }
            else if (userDbSoccerPlayer.second == Role.SUBSTITUTE)
            {
                score += userDbSoccerPlayer.first.score;
            }
        }

        return score;
    }

    public static void setUsersScores()
    {
        int score;
        foreach (var user in userDataMethods.getAllDbUsers())
        {
            score = calculateUserScore(UserConversion.convertdbUserToUser(user));
            userDataMethods.updateUserScore(user, score);
        }
    }

    public static List<User> getAllUsers()
    {
        return UserConversion.convertDbUsersListToUsersList(userDataMethods.getAllDbUsers());
    }

    public static void removeUser(User user)
    {
        userDataMethods.removeDbUser(UserConversion.convertUserToDbUser(user));
    }

    public static void deleteUsersSoccerPlayers()
    {
        foreach (var user in userDataMethods.getAllDbUsers())
        {
            userDataMethods.deleteUserSoccerPlayers(user);
            userDataMethods.resetDbUserCash(user);
        }
    }

    public static void addUserEmailConfirmation(string email, string OtpCode)
    {
        userDataMethods.addItemToDbUserEmailConfirmation(email, OtpCode);
    }

    public static void deleteUserEmailConfirmation(string email)
    {
        userDataMethods.deleteDbUserEmailConfirmation(email);
    }

    public static bool verifyEmailConfirmation(string email, string otpCode)
    {
        string realOtpCode = userDataMethods.getDbUserEmailConfirmationOtpCode(email);
        if (realOtpCode == null || realOtpCode != otpCode)
        {
            return false;
        }

        return true;
    }

    public static void updateUserPassword(string email, string password)
    {
        userDataMethods.updateDbUserPassword(email, password);
    }

    public static void purgeUserEmailConfirmations()
    {
        userDataMethods.purgeDbUserEmailConfirmations();
    }

    public static bool samenessOfTwoUsers(User firstUser, User secondUser)
    {
        return (firstUser.firstName == secondUser.firstName && firstUser.lastName == secondUser.lastName &&
                firstUser.password == secondUser.password);
    }
}