namespace WebApplication.BusinessLogic;

using DataAccess;

public class SoccerPlayer
{
    public int soccerPlayerId { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public int score { get; set; }
    public string team { get; set; }
    public double price { get; set; }
    public Position position { get; set; }
    public Role role { get; set; }
    public string photo { get; set; }
}