using WebApplication.BusinessLogic;
using WebApplication.DataAccess;

namespace WebApplication.BusinessLogic;

public class tempSoccerPlayer
{
    public int id { get; set; }
    public string first_name { get; set; }
    public string second_name { get; set; }
    public int total_points { get; set; }
    public int team { get; set; }

    public double now_cost { get; set; }

    // 1: GoalKeeper  2: Defenders  3: Midfielders  4: Forwards
    public int element_type { get; set; }
    public string photo { get; set; }
}

public class team
{
    public int id { get; set; }
    public string name { get; set; }
}

public class FFResponse
{
    public List<tempSoccerPlayer> elements { get; set; }
    public List<team> Teams { get; set; }
}

public class CallingFFApi
{
    public static string findTeamById(int id, List<team> teams)
    {
        foreach (var team in teams)
        {
            if (team.id == id)
            {
                return team.name;
            }
        }

        return "null";
    }

    public static Position FindPositionByNum(int n)
    {
        switch (n)
        {
            case 1:
                return Position.GOALKEEPER;
            case 2:
                return Position.DEFENDERS;
            case 3:
                return Position.MIDFIELDER;
            default:
                return Position.FORWARDS;
        }
    }
}