namespace WebApplication.BusinessLogic;

public class User
{
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string emailAddress { get; set; }
    public string userName { get; set; }
    public string password { get; set; }
    public int score { get; set; }
    public double cash { get; set; }
    public string OtpCode { get; set; }
    public DateTime OtpCodeExpTime { get; set; }
}