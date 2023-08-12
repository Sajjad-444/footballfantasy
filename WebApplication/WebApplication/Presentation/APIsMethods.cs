namespace WebApplication.Presentation;

using BusinessLogic;

public class APIsMethods
{
    private static ApiResponse buildApiResponse(bool success, int statusCode, string inputMessage,
        bool multipleMessages)
    {
        ApiResponse response = new ApiResponse();
        response.success = success;
        response.statusCode = statusCode;
        response.data = new List<messageInfo>();
        if (multipleMessages)
        {
            string[] messages = inputMessage.Split("#");
            foreach (var message in messages)
            {
                response.data.Add(
                    new messageInfo()
                    {
                        message = message
                    });
            }

            return response;
        }

        response.data = new List<messageInfo>()
        {
            new messageInfo()
            {
                message = inputMessage
            }
        };

        return response;
    }

    private static ApiResponse buildApiResponse(bool success, int statusCode, List<messageInfo> data)
    {
        ApiResponse response = new ApiResponse();
        response.success = success;
        response.statusCode = statusCode;
        response.data = data;
        return response;
    }

    private static ApiResponse buildApiResponse(bool success, int statusCode,
        List<SoccerPlayer> soccerPlayers, int pageCount)
    {
        GetSoccerPlayersApiResponse response = new GetSoccerPlayersApiResponse();
        response.success = success;
        response.statusCode = statusCode;
        response.soccerPlayers = soccerPlayers;
        response.pageCount = pageCount;
        return response;
    }

    private static ApiResponse buildApiResponse(bool success, int statusCode, User user)
    {
        GetUsersApiResponse response = new GetUsersApiResponse();
        response.success = success;
        response.statusCode = statusCode;
        response.users = new List<User>() { user };
        return response;
    }

    private static ApiResponse buildApiResponse(bool success, int statusCode, List<User> users,
        int pageCount)
    {
        GetUsersApiResponse response = new GetUsersApiResponse();
        response.success = success;
        response.statusCode = statusCode;
        response.users = users;
        response.pageCount = pageCount;
        return response;
    }

    private static object lockSignUp = new object();

    public static ApiResponse signUpByToken(string frontToken)
    {
        lock (lockSignUp)
        {
            UserLogic.deleteExpiredTempUsers();
            string userName = JwtManager.decodeToken(frontToken);
            if (userName == null)
            {
                return buildApiResponse(false, StatusCode.Unauthorized, "Please sign up", false);
            }

            User user = UserLogic.findTempUser(userName);
            if (user == null)
            {
                return buildApiResponse(false, StatusCode.NotFound, "Please sign up", false);
            }

            List<messageInfo> data = new List<messageInfo>();
            if (user.OtpCodeExpTime > DateTime.UtcNow)
            {
                return buildApiResponse(true, StatusCode.Ok,
                    "Enter your OtpCode#" + (user.OtpCodeExpTime - DateTime.UtcNow), true);
            }

            string OtpCode = OtpService.OtpCodeGenerator();
            if (!OtpService.sendOtpCode(user.emailAddress, OtpCode))
            {
                return buildApiResponse(false, StatusCode.ServiceUnavailable, "The Email wasn't sent successfully",
                    false);
            }

            user.OtpCode = OtpCode;
            user.OtpCodeExpTime = DateTime.UtcNow.AddMinutes(3);
            UserLogic.updateTempUserOtp(user);
            string token = JwtManager.generateToken(user);
            return buildApiResponse(true, StatusCode.Ok,
                "OtpCode sent successfully#" + (user.OtpCodeExpTime - DateTime.UtcNow) + "#" + token, true);
        }
    }

    public static ApiResponse signUp(User user)
    {
        lock (lockSignUp)
        {
            user.emailAddress = user.emailAddress.ToLower();
            UserLogic.deleteExpiredTempUsers();
            ApiResponse response = new ApiResponse();
            if (InputsValidation.signUpInputValidations(user)[0].message == "All inputs are valid")
            {
                switch (UserLogic.UserExistence(user.userName, user.emailAddress))
                {
                    case UsernameEmailExistence.UsernameAndEmailInTemp:
                        response = buildApiResponse(false, StatusCode.Conflict, "UserNameAndEmailInTemp", false);
                        break;
                    case UsernameEmailExistence.UsernameAndEmailInMain:
                        response = buildApiResponse(false, StatusCode.Conflict,
                            "Choose another emailAddress and userName", false);
                        break;
                    case UsernameEmailExistence.UsernameInMain:
                    case UsernameEmailExistence.UsernameInTemp:
                        response = buildApiResponse(false, StatusCode.Conflict, "Choose another userName", false);
                        break;
                    case UsernameEmailExistence.EmailInMain:
                    case UsernameEmailExistence.EmailInTemp:
                        response = buildApiResponse(false, StatusCode.Conflict, "Choose another email", false);
                        break;
                    default:
                        response = buildApiResponse(true, StatusCode.Ok, "Enter your OtpCode", false);
                        break;
                }

                if (!response.success && response.data[0].message == "UserNameAndEmailInTemp")
                {
                    User tempUser = UserLogic.findTempUser(user.userName);
                    if (!UserLogic.samenessOfTwoUsers(user, tempUser))
                    {
                        return buildApiResponse(false, StatusCode.Conflict,
                            "Choose another emailAddress and userName", false);
                    }

                    List<messageInfo> data = new List<messageInfo>();
                    if (tempUser.OtpCodeExpTime > DateTime.UtcNow)
                    {
                        return buildApiResponse(true, StatusCode.Ok,
                            "Enter your OtpCode#" + (tempUser.OtpCodeExpTime - DateTime.UtcNow), true);
                    }

                    string OtpCode = OtpService.OtpCodeGenerator();
                    if (!OtpService.sendOtpCode(user.emailAddress, OtpCode))
                    {
                        return buildApiResponse(false, StatusCode.ServiceUnavailable,
                            "The Email wasn't sent successfully", false);
                    }

                    user.OtpCode = OtpCode;
                    user.OtpCodeExpTime = DateTime.UtcNow.AddMinutes(3);
                    UserLogic.updateTempUserOtp(user);
                    string token = JwtManager.generateToken(user);
                    return buildApiResponse(true, StatusCode.Ok,
                        "OtpCode sent successfully#" + (user.OtpCodeExpTime - DateTime.UtcNow) + "#" + token, true);
                }

                if (response.success)
                {
                    string OtpCode = OtpService.OtpCodeGenerator();
                    if (OtpService.sendOtpCode(user.emailAddress, OtpCode))
                    {
                        user.OtpCode = OtpCode;
                        user.OtpCodeExpTime = DateTime.UtcNow.AddMinutes(3);
                        UserLogic.addUserToTempTable(user);
                        string token = JwtManager.generateToken(user);
                        return buildApiResponse(true, StatusCode.Ok,
                            "Enter your OtpCode#" + (user.OtpCodeExpTime - DateTime.UtcNow) + "#" + token, true);
                    }

                    return buildApiResponse(false, StatusCode.ServiceUnavailable,
                        "The Email wasn't sent successfully", false);
                }

                return response;
            }

            response = buildApiResponse(false, StatusCode.BadRequest, InputsValidation.signUpInputValidations(user));
            return response;
        }
    }

    public static ApiResponse frontTokenAuthentication(string frontToken)
    {
        UserLogic.deleteExpiredTempUsers();
        string userName = JwtManager.decodeToken(frontToken);
        if (userName == null)
        {
            return buildApiResponse(false, StatusCode.Unauthorized, "You should Sign In with credentials", false);
        }

        User user = UserLogic.findMainUserByUserName(userName);
        if (user == null)
        {
            return buildApiResponse(false, StatusCode.NotFound, "You should Sign In with credentials", false);
        }

        return buildApiResponse(true, StatusCode.Ok, "Signed in Successfully", false);
    }

    public static ApiResponse loginByCredentials(string input, string password)
    {
        UserLogic.deleteExpiredTempUsers();
        bool isEmail = InputsValidation.emailAddressValidation(input);
        if (!InputsValidation.passwordValidation(password) &&
            (!isEmail && !InputsValidation.userNameValidation(input)))
        {
            return buildApiResponse(false, StatusCode.BadRequest, "The UserName/Email and Password are invalid", false);
        }

        if (!isEmail && !InputsValidation.userNameValidation(input))
        {
            return buildApiResponse(false, StatusCode.BadRequest, "The UserName/Email is invalid", false);
        }

        if (!(InputsValidation.passwordValidation(password)))
        {
            return buildApiResponse(false, StatusCode.BadRequest, "Password is invalid", false);
        }

        if (isEmail)
        {
            input = input.ToLower();
            if (UserLogic.UserExistence("", input) == UsernameEmailExistence.EmailInMain)
            {
                User user = UserLogic.findMainUserByEmail(input);
                if (user.password == password)
                {
                    string token = JwtManager.generateToken(user);
                    return buildApiResponse(true, StatusCode.Ok, "Signed in successfully#" + token, true);
                }

                return buildApiResponse(false, StatusCode.Unauthorized, "Password is incorrect", false);
            }

            return buildApiResponse(false, StatusCode.NotFound, "There is no user with this UserName/Email", false);
        }

        if (UserLogic.UserExistence(input, "") == UsernameEmailExistence.UsernameInMain)
        {
            User user = UserLogic.findMainUserByUserName(input);
            if (user.password == password)
            {
                string token = JwtManager.generateToken(user);
                return buildApiResponse(true, StatusCode.Ok, "Signed in successfully#" + token, true);
            }

            return buildApiResponse(false, StatusCode.Unauthorized, "Password is incorrect", false);
        }

        return buildApiResponse(false, StatusCode.NotFound, "There is no user with this UserName/Email", false);
    }

    public static ApiResponse resendOtpCode(string userName)
    {
        if (!InputsValidation.userNameValidation(userName))
        {
            return buildApiResponse(false, StatusCode.NoContent, "UserName is invalid", false);
        }

        User user = UserLogic.findTempUser(userName);
        if (user == null)
        {
            return buildApiResponse(false, StatusCode.NotFound, "Please Sign up again", false);
        }

        if (user.OtpCodeExpTime > DateTime.UtcNow)
        {
            return buildApiResponse(false, StatusCode.TooManyRequests, "Please wait for about 3 minutes", false);
        }

        if (user.OtpCodeExpTime.AddDays(7) >= DateTime.UtcNow)
        {
            string OtpCode = OtpService.OtpCodeGenerator();
            if (!OtpService.sendOtpCode(user.emailAddress, OtpCode))
            {
                return buildApiResponse(false, StatusCode.ServiceUnavailable, "The Email wasn't sent successfully",
                    false);
            }

            user.OtpCode = OtpCode;
            user.OtpCodeExpTime = DateTime.UtcNow.AddMinutes(3);
            UserLogic.updateTempUserOtp(user);
            string token = JwtManager.generateToken(user);
            return buildApiResponse(true, StatusCode.Ok,
                "OtpCode sent successfully#" + (user.OtpCodeExpTime - DateTime.UtcNow) + "#" + token, true);
        }

        UserLogic.deleteTempUser(user);
        return buildApiResponse(false, StatusCode.RequestTimeout, "Please sign up again", false);
    }

    public static ApiResponse otpCodeVerification(string otpCode, string userName)
    {
        if (!InputsValidation.userNameValidation(userName))
        {
            return buildApiResponse(false, StatusCode.NoContent, "UserName is invalid", false);
        }

        User user = UserLogic.findTempUser(userName);
        if (user == null)
        {
            return buildApiResponse(false, StatusCode.NotFound, "The user with this userName doesn't exist", false);
        }

        if (!OtpService.OtpCodeVerification(otpCode, userName))
        {
            return buildApiResponse(false, StatusCode.BadRequest, "The entered code isn't correct", false);
        }

        UserLogic.addUserToMainTable(user);
        UserLogic.deleteTempUser(user);
        return buildApiResponse(true, StatusCode.Ok, "Signed Up successfully", false);
    }

    public static ApiResponse addSoccerPlayer(string frontToken, SoccerPlayer soccerPlayer)
    {
        if (SoccerPlayersTableUpdateScheduler.SoccerPlayersTableUpdate)
        {
            return SoccerPlayersTableUpdateScheduler.soccerPlayersUpdateResponse();
        }

        if (soccerPlayer.role == Role.NOTCHOSEN)
        {
            return buildApiResponse(false, StatusCode.BadRequest, "The role of the soccerPlayer is not chosen", false);
        }

        string userName = JwtManager.decodeToken(frontToken);
        if (userName == null)
        {
            return buildApiResponse(false, StatusCode.Unauthorized, "You should Sign In with credentials", false);
        }

        User user = UserLogic.findMainUserByUserName(userName);
        if (user == null)
        {
            return buildApiResponse(false, StatusCode.NotFound, "You should Sign In with credentials", false);
        }

        if (SoccerPlayerLogic.soccerPlayerExistence(user, soccerPlayer.soccerPlayerId, Role.MAIN) ||
            SoccerPlayerLogic.soccerPlayerExistence(user, soccerPlayer.soccerPlayerId, Role.SUBSTITUTE))
        {
            return buildApiResponse(false, StatusCode.BadRequest,
                "The soccerPlayer is already existed in your list", false);
        }

        List<messageInfo> verificationResponce = SoccerPlayerLogic.soccerPlayerVerification(user, soccerPlayer);
        if (verificationResponce[0].message != "OK")
        {
            return buildApiResponse(false, StatusCode.BadRequest, verificationResponce);
        }

        SoccerPlayerLogic.addSoccerPlayerToUser(user, soccerPlayer);
        UserLogic.increaseUserCash(user, -1 * soccerPlayer.price);
        return buildApiResponse(true, StatusCode.Ok, "The soccerPlayer is added successfully", false);
    }

    public static ApiResponse removeSoccerPlayer(string frontToken, SoccerPlayer soccerPlayer)
    {
        if (SoccerPlayersTableUpdateScheduler.SoccerPlayersTableUpdate)
        {
            return SoccerPlayersTableUpdateScheduler.soccerPlayersUpdateResponse();
        }

        string userName = JwtManager.decodeToken(frontToken);
        if (userName == null)
        {
            return buildApiResponse(false, StatusCode.Unauthorized, "You Should Sign In With Credentials", false);
        }

        User user = UserLogic.findMainUserByUserName(userName);
        if (user == null)
        {
            return buildApiResponse(false, StatusCode.NotFound, "You Should Sign In With Credentials", false);
        }

        if (!SoccerPlayerLogic.soccerPlayerExistence(user, soccerPlayer.soccerPlayerId, soccerPlayer.role))
        {
            return buildApiResponse(false, StatusCode.BadRequest, "The SoccerPlayer doesn't exist", false);
        }

        SoccerPlayerLogic.deleteSoccerPlayer(user, soccerPlayer);
        UserLogic.increaseUserCash(user, soccerPlayer.price);
        return buildApiResponse(true, StatusCode.Ok, "SoccerPlayer is removed Successfully", false);
    }

    public static ApiResponse swapSoccerPlayersRole(string frontToken, List<SoccerPlayer> soccerPlayers)
    {
        SoccerPlayer soccerPlayer1 = soccerPlayers[0];
        SoccerPlayer soccerPlayer2 = soccerPlayers[1];

        string userName = JwtManager.decodeToken(frontToken);
        if (userName == null)
        {
            return buildApiResponse(false, StatusCode.Unauthorized, "You should Sign In with credentials", false);
        }

        User user = UserLogic.findMainUserByUserName(userName);
        if (user == null)
        {
            return buildApiResponse(false, StatusCode.NotFound, "You should Sign In with credentials", false);
        }

        if (!SoccerPlayerLogic.soccerPlayerExistence(user, soccerPlayer1.soccerPlayerId, soccerPlayer1.role) ||
            !SoccerPlayerLogic.soccerPlayerExistence(user, soccerPlayer2.soccerPlayerId, soccerPlayer2.role))
        {
            return buildApiResponse(false, StatusCode.BadRequest, "The SoccerPlayer doesn't exist", false);
        }

        if (soccerPlayer1.role == soccerPlayer2.role)
        {
            return buildApiResponse(false, StatusCode.BadRequest,
                "The role of these two soccerPlayers is the same", false);
        }

        SoccerPlayerLogic.swapSoccerPlayersRole(user, soccerPlayer1.soccerPlayerId, soccerPlayer2.soccerPlayerId);
        return buildApiResponse(true, StatusCode.Ok, "The Role Swapped successfully", false);
    }

    public static ApiResponse filterSoccerPlayers(string name = "", double priceStart = -1,
        double priceEnd = -1, int scoreStart = -1, int scoreEnd = -1, Position position = Position.NOTCHOSEN,
        Trend trend = Trend.NOTCHOSEN, TrendOf trendOf = TrendOf.NOTCHOSEN, int pageNum = 1)
    {
        if (SoccerPlayersTableUpdateScheduler.SoccerPlayersTableUpdate)
        {
            return SoccerPlayersTableUpdateScheduler.soccerPlayersUpdateResponse();
        }

        List<SoccerPlayer> temp = new List<SoccerPlayer>();
        if (name == "" && (priceStart == -1 || priceEnd == -1) && (scoreStart == -1 || scoreEnd == -1) &&
            position == Position.NOTCHOSEN && trend == Trend.NOTCHOSEN && trendOf == TrendOf.NOTCHOSEN)
        {
            temp = SoccerPlayerLogic.getAllSoccerPlayers();
        }
        else
        {
            temp = SoccerPlayerLogic.getFilteredSoccerPlayers(name, priceStart, priceEnd, scoreStart,
                scoreEnd, position, trend, trendOf);
        }

        SoccerPlayerLogic.sortSoccerPlayers(trend, trendOf, ref temp);
        int pageCount = (int)Math.Ceiling(temp.Count / 20.0);
        if (!(1 <= pageNum && pageNum <= pageCount))
        {
            pageNum = 1;
        }

        List<SoccerPlayer> soccerPlayers =
            temp.GetRange((pageNum - 1) * 20, Math.Min(20, temp.Count - (pageNum - 1) * 20));
        return buildApiResponse(true, StatusCode.Ok, soccerPlayers, pageCount);
    }

    public static ApiResponse getUserSoccerPlayers(string frontToken)
    {
        if (SoccerPlayersTableUpdateScheduler.SoccerPlayersTableUpdate)
        {
            return SoccerPlayersTableUpdateScheduler.soccerPlayersUpdateResponse();
        }

        string userName = JwtManager.decodeToken(frontToken);
        if (userName == null)
        {
            return buildApiResponse(false, StatusCode.Unauthorized, "You should Sign In with credentials", false);
        }

        User user = UserLogic.findMainUserByUserName(userName);
        if (user == null)
        {
            return buildApiResponse(false, StatusCode.NotFound, "You should Sign In with credentials", false);
        }

        List<SoccerPlayer> userSoccerPlayers = SoccerPlayerLogic.getUserSoccerPlayers(user);

        return buildApiResponse(true, StatusCode.Ok, userSoccerPlayers, 1);
    }

    private static List<User> safeUsers(List<User> users)
    {
        List<User> newUsers = new List<User>();
        foreach (var user in users)
        {
            newUsers.Add(new User()
            {
                userName = user.userName,
                score = user.score
            });
        }

        return newUsers;
    }

    public static ApiResponse getUsers(int pageNum = 1)
    {
        if (SoccerPlayersTableUpdateScheduler.SoccerPlayersTableUpdate)
        {
            return SoccerPlayersTableUpdateScheduler.soccerPlayersUpdateResponse();
        }

        List<User> allUsers = UserLogic.getAllUsers();
        allUsers.Sort((x, y) => y.score.CompareTo(x.score));
        int pageCount = (int)Math.Ceiling(allUsers.Count / 20.0);
        if (!(1 <= pageNum && pageNum <= pageCount))
        {
            pageNum = 1;
        }

        List<User> users = allUsers.GetRange((pageNum - 1) * 20, Math.Min(20, allUsers.Count - (pageNum - 1) * 20));
        return buildApiResponse(true, StatusCode.Ok, safeUsers(users), pageCount);
    }

    public static ApiResponse deleteAccount(string frontToken)
    {
        string userName = JwtManager.decodeToken(frontToken);
        if (userName == null)
        {
            return buildApiResponse(false, StatusCode.Unauthorized, "You should Sign In with credentials", false);
        }

        User user = UserLogic.findMainUserByUserName(userName);
        if (user == null)
        {
            return buildApiResponse(false, StatusCode.NotFound, "You should Sign In with credentials", false);
        }

        UserLogic.removeUser(user);
        return buildApiResponse(true, StatusCode.Ok, "Account deleted successfully", false);
    }


    public static ApiResponse sendConfirmationEmail(string email)
    {
        email = email.ToLower();
        if (SoccerPlayersTableUpdateScheduler.SoccerPlayersTableUpdate)
        {
            return SoccerPlayersTableUpdateScheduler.soccerPlayersUpdateResponse();
        }

        if (UserLogic.UserExistence("", email) != UsernameEmailExistence.EmailInMain)
        {
            return buildApiResponse(false, StatusCode.NotFound, "No user has signed Up successfully with this email",
                false);
        }

        string OtpCode = OtpService.OtpCodeGenerator();
        if (!OtpService.sendOtpCode(email, OtpCode))
        {
            return buildApiResponse(false, StatusCode.ServiceUnavailable, "The Email wasn't sent successfully", false);
        }

        UserLogic.deleteUserEmailConfirmation(email);
        UserLogic.addUserEmailConfirmation(email, OtpCode);
        return buildApiResponse(true, StatusCode.Ok, "Email sent successfully", false);
    }

    public static ApiResponse setNewPassword(string email, string otpCode, string newPassword)
    {
        email = email.ToLower();
        if (SoccerPlayersTableUpdateScheduler.SoccerPlayersTableUpdate)
        {
            return SoccerPlayersTableUpdateScheduler.soccerPlayersUpdateResponse();
        }

        if (UserLogic.UserExistence("", email) != UsernameEmailExistence.EmailInMain)
        {
            return buildApiResponse(false, StatusCode.NotFound,
                "No user has signed Up successfully with this email", false);
        }

        if (!UserLogic.verifyEmailConfirmation(email, otpCode))
        {
            return buildApiResponse(false, StatusCode.Unauthorized, "The entered code isn't correct", false);
        }

        if (!InputsValidation.passwordValidation(newPassword))
        {
            return buildApiResponse(false, StatusCode.BadRequest, "The eneterd Password is invalid", false);
        }

        UserLogic.updateUserPassword(email, newPassword);
        UserLogic.deleteUserEmailConfirmation(email);
        return buildApiResponse(true, StatusCode.Ok, "Password changed successfully", false);
    }


    public static ApiResponse getUserInfo(string frontToken)
    {
        if (SoccerPlayersTableUpdateScheduler.SoccerPlayersTableUpdate)
        {
            return SoccerPlayersTableUpdateScheduler.soccerPlayersUpdateResponse();
        }

        string userName = JwtManager.decodeToken(frontToken);
        if (userName == null)
        {
            return buildApiResponse(false, StatusCode.Unauthorized, "You Should Sign In With Credentials", false);
        }

        User mainUser = UserLogic.findMainUserByUserName(userName);
        if (mainUser == null)
        {
            return buildApiResponse(false, StatusCode.NotFound, "You Should Sign In With Credentials", false);
        }

        User user = new User()
        {
            userName = mainUser.userName,
            score = mainUser.score
        };
        return buildApiResponse(true, StatusCode.Ok, user);
    }
}