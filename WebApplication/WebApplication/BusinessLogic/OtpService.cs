namespace WebApplication.Presentation;

using BusinessLogic;
using System.Net;
using System.Net.Mail;

public class OtpService
{
    private static string smtpServer = "smtp.gmail.com";
    private static int smtpPort = 25;
    private static string smtpUsername = "footballfantasyf59@gmail.com";
    private static string smtpPassword = "ehgajmvizcjqzfbb";

    public static string OtpCodeGenerator()
    {
        Random random = new Random();
        return random.Next(100000, 999999).ToString();
    }

    private static bool SendEmail(string clientEmail, string body)
    {
        try
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(smtpUsername);
            message.To.Add(clientEmail);
            message.Subject = "Verification Code From FootabllFantasy";
            message.Body = body;

            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
            smtpClient.EnableSsl = true;

            smtpClient.Send(message);
        }
        catch (Exception ex)
        {
            return false;
        }

        return true;
    }

    public static bool sendOtpCode(string emailAddress, string OtpCode)
    {
        if (!SendEmail(emailAddress, "Hey there.\n Your Verification Code is : " + OtpCode))
        {
            return false;
        }

        return true;
    }

    public static bool OtpCodeVerification(string OtpCode, string userName)
    {
        User user = UserLogic.findTempUser(userName);
        if (user.OtpCodeExpTime > DateTime.UtcNow && user.OtpCode == OtpCode)
        {
            return true;
        }

        return false;
    }
}