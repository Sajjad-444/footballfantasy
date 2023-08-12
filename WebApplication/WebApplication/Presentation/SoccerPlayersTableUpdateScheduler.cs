namespace WebApplication.Presentation;

using BusinessLogic;
using ServiceStack;
using System;
using System.Timers;

public class SoccerPlayersTableUpdateScheduler
{
    public static bool SoccerPlayersTableUpdate;
    private Timer timer;

    public void Start()
    {
        DayOfWeek today = DayOfWeek.Friday;
        timer = new Timer(TimeSpan.FromDays(7).TotalMilliseconds);
        timer.Elapsed += updateSoccerPlayersTable;
        timer.AutoReset = true;
        timer.Start();
    }

    public static void updateSoccerPlayersTable(object sender, ElapsedEventArgs e)
    {
        UserLogic.setUsersScores();
        UserLogic.deleteUsersSoccerPlayers();
        UserLogic.purgeUserEmailConfirmations();
        SoccerPlayersTableUpdate = true;
        string url = "https://fantasy.premierleague.com/api/bootstrap-static/";
        FFResponse response = url.GetJsonFromUrl().FromJson<FFResponse>();
        SoccerPlayerLogic.purgeSoccerPlayersTable();
        foreach (var tempSoccerPlayer in response.elements)
        {
            SoccerPlayer soccerPlayer = new SoccerPlayer()
            {
                soccerPlayerId = tempSoccerPlayer.id,
                firstName = tempSoccerPlayer.first_name,
                lastName = tempSoccerPlayer.second_name,
                score = tempSoccerPlayer.total_points,
                team = CallingFFApi.findTeamById(tempSoccerPlayer.team, response.Teams),
                price = tempSoccerPlayer.now_cost / 10.0,
                position = CallingFFApi.FindPositionByNum(tempSoccerPlayer.element_type),
                photo = tempSoccerPlayer.photo
            };
            SoccerPlayerLogic.addSoccerPlayerToDataBase(soccerPlayer);
        }

        SoccerPlayersTableUpdate = false;
    }

    public static ApiResponse soccerPlayersUpdateResponse()
    {
        ApiResponse response = new ApiResponse();
        response.success = false;
        response.statusCode = StatusCode.ServiceUnavailable;
        response.data = new List<messageInfo>()
        {
            new messageInfo()
            {
                message =
                    "Please try again in a few seconds, the soccerPlayers List is Updating and The User's Scores are getting set."
            }
        };
        return response;
    }
}