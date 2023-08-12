namespace WebApplication.Presentation;

using BusinessLogic;

public class InputsValidation
{
    public static bool firstNameValidation(string firstName)
    {
        string str = "abcdefghijklmnopqrtsuvwxyzABCDEFGEHIJKLMNOPQRSTUVWXYZ";
        if (firstName.Length < 1)
        {
            return false;
        }

        for (int i = 0; i < firstName.Length; i++)
        {
            if (!str.Contains(firstName[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool lastNameValidation(string lastName)
    {
        string str = "abcdefghijklmnopqrtsuvwxyzABCDEFGEHIJKLMNOPQRSTUVWXYZ";
        if (lastName.Length < 1)
        {
            return false;
        }

        for (int i = 0; i < lastName.Length; i++)
        {
            if (!str.Contains(lastName[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool userNameValidation(string userName)
    {
        string str = "abcdefghijklmnopqrtsuvwxyzABCDEFGEHIJKLMNOPQRSTUVWXYZ0123456789-_";
        if (userName.Length < 1)
        {
            return false;
        }

        for (int i = 0; i < userName.Length; i++)
        {
            if (!str.Contains(userName[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool passwordCapitalLetterValidation(string password)
    {
        string str = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        foreach (var variable in password)
        {
            if (str.Contains(variable))
            {
                return true;
            }
        }

        return false;
    }

    public static bool passwordSmallLetterValidation(string password)
    {
        string str = "abcdefghijklmnopqrstuvwxyz";
        foreach (var variable in password)
        {
            if (str.Contains(variable))
            {
                return true;
            }
        }

        return false;
    }

    public static bool passwordSpecialLettersValidations(string password)
    {
        string str = "!@#$%^&*()-_=+,<.>/?\\|;:'\"";
        foreach (var variable in password)
        {
            if (str.Contains(variable))
            {
                return true;
            }
        }

        return false;
    }

    public static bool passwordNumsValidations(string password)
    {
        string str = "1234567890";
        foreach (var variable in password)
        {
            if (str.Contains(variable))
            {
                return true;
            }
        }

        return false;
    }

    public static bool passwordValidation(string password)
    {
        string str = "!@#ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-_=+,<.>/?\\|;:'\"";
        if (password.Length < 8)
        {
            return false;
        }

        for (int i = 0; i < password.Length; i++)
        {
            if (!str.Contains(password[i]))
            {
                return false;
            }
        }

        return passwordSmallLetterValidation(password) && passwordCapitalLetterValidation(password) &&
               passwordSpecialLettersValidations(password) && passwordNumsValidations(password);
    }

    public static bool domainEmailAddressValidation(string domainEmailAddress)
    {
        List<string> validDomains = new List<string>
        {
            "gmail.com",
            "yahoo.com",
            "hotmail.com",
            "outlook.com",
            "aol.com",
            "icloud.com",
            "msn.com",
            "live.com",
            "zoho.com",
            "yandex.com",
            "protonmail.com",
            "mail.com",
            "gmx.com",
            "inbox.com",
            "fastmail.com",
            "att.net",
            "rocketmail.com",
            "cox.net",
            "sbcglobal.net",
            "me.com",
            "mac.com",
            "verizon.net",
            "bellsouth.net",
            "ymail.com",
            "optonline.net",
            "earthlink.net",
            "charter.net",
            "ntlworld.com",
            "btinternet.com"
        };
        foreach (var domainPart in validDomains)
        {
            if (domainEmailAddress == domainPart)
            {
                return true;
            }
        }

        return false;
    }

    public static bool localEmailAddressValidation(string localEmailAddress)
    {
        string str = "abcdefghijklmnopqrtsuvwxyzABCDEFGEHIJKLMNOPQRSTUVWXYZ0123456789.-_";
        for (int i = 0; i < localEmailAddress.Length; i++)
        {
            if (!str.Contains(localEmailAddress[i]))
            {
                return false;
            }

            if (localEmailAddress[i] == '.' || localEmailAddress[i] == '-' || localEmailAddress[i] == '_')
            {
                if (localEmailAddress[i + 1] == '.' || localEmailAddress[i + 1] == '-' ||
                    localEmailAddress[i + 1] == '_')
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static bool emailAddressValidation(string emailAddress)
    {
        if (emailAddress.Length < 3)
        {
            return false;
        }

        string[] email = emailAddress.Split("@");
        if (email.Length != 2)
        {
            return false;
        }

        return localEmailAddressValidation(email[0]) && domainEmailAddressValidation(email[1]);
    }

    //MainMethod
    public static List<messageInfo> signUpInputValidations(User user)
    {
        List<messageInfo> invalidInput = new List<messageInfo>();
        if (!firstNameValidation(user.firstName))
        {
            invalidInput.Add(
                new messageInfo()
                {
                    message = "FirstName is invalid"
                }
            );
        }

        if (!lastNameValidation(user.lastName))
        {
            invalidInput.Add(
                new messageInfo()
                {
                    message = "LastName is invalid"
                }
            );
        }

        if (!userNameValidation(user.userName))
        {
            invalidInput.Add(
                new messageInfo()
                {
                    message = "UserName is invalid"
                }
            );
        }

        if (!passwordValidation(user.password))
        {
            invalidInput.Add(
                new messageInfo()
                {
                    message = "Password is invalid"
                }
            );
        }

        if (!emailAddressValidation(user.emailAddress))
        {
            invalidInput.Add(
                new messageInfo()
                {
                    message = "EmailAddress is invalid"
                }
            );
        }

        if (invalidInput.Count == 0)
        {
            invalidInput.Add(
                new messageInfo()
                {
                    message = "All inputs are valid"
                }
            );
        }

        return invalidInput;
    }
}