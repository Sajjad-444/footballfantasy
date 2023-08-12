namespace WebApplication.BusinessLogic;

using DataAccess;

public class SoccerPlayerConversion
{
    public static DbSoccerPlayer ConvertSoccerPlayerToDbSoccerPlayer(SoccerPlayer soccerPlayer)
    {
        if (soccerPlayer == null)
        {
            return null;
        }

        return new DbSoccerPlayer()
        {
            dbSoccerPlayerId = soccerPlayer.soccerPlayerId,
            firstName = soccerPlayer.firstName,
            lastName = soccerPlayer.lastName,
            position = soccerPlayer.position,
            price = soccerPlayer.price,
            score = soccerPlayer.score,
            team = soccerPlayer.team,
            photo = soccerPlayer.photo
        };
    }

    public static SoccerPlayer ConvertDbSoccerPlayerToSoccerPlayer(DbSoccerPlayer dbSoccerPlayer,
        Role role = Role.NOTCHOSEN)
    {
        if (dbSoccerPlayer == null)
        {
            return null;
        }

        return new SoccerPlayer()
        {
            soccerPlayerId = dbSoccerPlayer.dbSoccerPlayerId,
            firstName = dbSoccerPlayer.firstName,
            lastName = dbSoccerPlayer.lastName,
            score = dbSoccerPlayer.score,
            team = dbSoccerPlayer.team,
            price = dbSoccerPlayer.price,
            position = dbSoccerPlayer.position,
            role = role,
            photo = dbSoccerPlayer.photo
        };
    }

    public static List<SoccerPlayer> ConvertDbSoccerPlayersListToSoccerPlayersList(List<DbSoccerPlayer> dbSoccerPlayers)
    {
        List<SoccerPlayer> result = new List<SoccerPlayer>();
        foreach (var dbSoccerPlayer in dbSoccerPlayers)
        {
            result.Add(ConvertDbSoccerPlayerToSoccerPlayer(dbSoccerPlayer));
        }

        return result;
    }
}