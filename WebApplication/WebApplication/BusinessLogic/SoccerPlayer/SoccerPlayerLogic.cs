using WebApplication.DataAccess;
using WebApplication.Presentation;

namespace WebApplication.BusinessLogic;

public class SoccerPlayerLogic
{
    public static void addSoccerPlayerToUser(User user, SoccerPlayer soccerPlayer)
    {
        SoccerPlayerDataMethods.addSoccerPlayerToDbUser(UserConversion.convertUserToDbUser(user),
            SoccerPlayerConversion.ConvertSoccerPlayerToDbSoccerPlayer(soccerPlayer), soccerPlayer.role);
    }

    public static void addSoccerPlayerToDataBase(SoccerPlayer soccerPlayer)
    {
        SoccerPlayerDataMethods.addSoccerPlayerToDbSoccerPlayerTable(
            SoccerPlayerConversion.ConvertSoccerPlayerToDbSoccerPlayer(soccerPlayer));
    }

    public static void purgeSoccerPlayersTable()
    {
        SoccerPlayerDataMethods.deleteAllDbSoccerPlayersFromTable();
    }

    public static bool soccerPlayerExistence(User user, int soccerPlayerId, Role role)
    {
        return SoccerPlayerDataMethods.soccerPlayerExistence(UserConversion.convertUserToDbUser(user), soccerPlayerId,
            role);
    }

    public static bool verifyPurchasingPower(User user, SoccerPlayer soccerPlayer)
    {
        return user.cash >= soccerPlayer.price;
    }

    public static bool verifySoccerPlayerNumber(User user, SoccerPlayer soccerPlayer)
    {
        return SoccerPlayerDataMethods.countUserSoccerPlayers(UserConversion.convertUserToDbUser(user)) < 15;
    }

    public static bool verifySoccerPlayerRole(User user, SoccerPlayer soccerPlayer)
    {
        switch (soccerPlayer.role)
        {
            case Role.MAIN:
                return SoccerPlayerDataMethods.countUserSoccerPlayersByRole(UserConversion.convertUserToDbUser(user),
                    soccerPlayer.role) < 11;
            case Role.SUBSTITUTE:
                return SoccerPlayerDataMethods.countUserSoccerPlayersByRole(UserConversion.convertUserToDbUser(user),
                    soccerPlayer.role) < 4;
            default:
                return false;
        }
    }

    public static bool verifySoccerPlayerPosition(User user, SoccerPlayer soccerPlayer)
    {
        switch (soccerPlayer.position)
        {
            case Position.MIDFIELDER:
                return SoccerPlayerDataMethods.countUserSoccerPlayersByPosition
                    (UserConversion.convertUserToDbUser(user), soccerPlayer.position) < 5;
            case Position.DEFENDERS:
                return SoccerPlayerDataMethods.countUserSoccerPlayersByPosition
                    (UserConversion.convertUserToDbUser(user), soccerPlayer.position) < 5;
            case Position.FORWARDS:
                return SoccerPlayerDataMethods.countUserSoccerPlayersByPosition
                    (UserConversion.convertUserToDbUser(user), soccerPlayer.position) < 3;
            default:
                return SoccerPlayerDataMethods.countUserSoccerPlayersByPosition
                    (UserConversion.convertUserToDbUser(user), soccerPlayer.position) < 2;
        }
    }

    public static bool verifySoccerPlayerTeam(User user, SoccerPlayer soccerPlayer)
    {
        return SoccerPlayerDataMethods.countUserSoccerPlayersByTeam
            (UserConversion.convertUserToDbUser(user), soccerPlayer.team) < 3;
    }

    public static List<messageInfo> soccerPlayerVerification(User user, SoccerPlayer soccerPlayer)
    {
        List<messageInfo> messages = new List<messageInfo>();
        if (!verifyPurchasingPower(user, soccerPlayer))
        {
            messages.Add(
                new messageInfo()
                {
                    message = "Account credit isn't enough"
                });
        }

        if (!verifySoccerPlayerNumber(user, soccerPlayer))
        {
            messages.Add(
                new messageInfo()
                {
                    message = "More than 15 soccerPlayers can't be chosen"
                });
        }

        if (!verifySoccerPlayerRole(user, soccerPlayer))
        {
            messages.Add(
                new messageInfo()
                {
                    message = "11 main players and 4 substitute players at most can be chosen"
                });
        }

        if (!verifySoccerPlayerPosition(user, soccerPlayer))
        {
            messages.Add(
                new messageInfo()
                {
                    message = "5 midfielders, 5 defenders, 3 forwards and 2 goalKeepers at most can be chosen"
                });
        }

        if (!verifySoccerPlayerTeam(user, soccerPlayer))
        {
            messages.Add(
                new messageInfo()
                {
                    message = "3 soccerPlayers at most can be chosen of a specific team"
                });
        }

        if (messages.Count == 0)
        {
            messages.Add(
                new messageInfo()
                {
                    message = "OK"
                });
        }

        return messages;
    }

    public static void deleteSoccerPlayer(User User, SoccerPlayer SoccerPlayer)
    {
        SoccerPlayerDataMethods.deleteSoccerPlayer(UserConversion.convertUserToDbUser(User),
            SoccerPlayerConversion.ConvertSoccerPlayerToDbSoccerPlayer(SoccerPlayer));
    }

    public static void swapSoccerPlayersRole(User user, int soccerPlayer1Id, int soccerPlayer2Id)
    {
        SoccerPlayerDataMethods.swapSoccerPlayersRole(UserConversion.convertUserToDbUser(user), soccerPlayer1Id,
            soccerPlayer2Id);
    }

    private static List<SoccerPlayer> extractCommonSoccerPlayers(List<List<SoccerPlayer>> soccerPlayersLists)
    {
        List<SoccerPlayer> result = new List<SoccerPlayer>();
        if (soccerPlayersLists.Count == 0)
        {
            return result;
        }

        result = soccerPlayersLists[0];
        for (int i = 0; i < soccerPlayersLists.Count - 1; i++)
        {
            result = result.Intersect(soccerPlayersLists[i + 1]).ToList();
        }

        return result;
    }

    private static List<List<SoccerPlayer>> initialTempFilters(string name, double priceStart, double priceEnd,
        int scoreStart, int scoreEnd, Position position) // trend:  1= Ascending  0= Descending
    {
        List<List<SoccerPlayer>> result = new List<List<SoccerPlayer>>();
        if (priceEnd != -1 && priceStart != -1)
        {
            result.Add(SoccerPlayerConversion.ConvertDbSoccerPlayersListToSoccerPlayersList(
                SoccerPlayerDataMethods.filterByPrice(priceStart, priceEnd)));
        }

        if (scoreStart != -1 && scoreEnd != -1)
        {
            result.Add(SoccerPlayerConversion.ConvertDbSoccerPlayersListToSoccerPlayersList(
                SoccerPlayerDataMethods.filterByScore(scoreStart, scoreEnd)));
        }

        if (name != "")
        {
            string[] nameParts = name.ToLower().Split(" ");
            List<List<SoccerPlayer>> soccerPlayersLists = new List<List<SoccerPlayer>>();
            foreach (var namePart in nameParts)
            {
                soccerPlayersLists.Add(
                    SoccerPlayerConversion.ConvertDbSoccerPlayersListToSoccerPlayersList(
                        SoccerPlayerDataMethods.filterByName(namePart)));
            }

            result.Add(extractCommonSoccerPlayers(soccerPlayersLists));
        }

        if (position != Position.NOTCHOSEN)
        {
            result.Add(SoccerPlayerConversion.ConvertDbSoccerPlayersListToSoccerPlayersList(
                SoccerPlayerDataMethods.filterByPosition(position)));
        }

        return result;
    }

    public static List<SoccerPlayer> getFilteredSoccerPlayers(string name, double priceStart, double priceEnd,
        int scoreStart, int scoreEnd, Position position, Trend trend, TrendOf trendOf)
    {
        List<List<SoccerPlayer>> filters =
            initialTempFilters(name, priceStart, priceEnd, scoreStart, scoreEnd, position);
        List<SoccerPlayer> filteredList = new List<SoccerPlayer>();
        if (filters.Count == 0)
        {
            return filteredList;
        }

        int j;
        for (int i = 0; i < filters[0].Count; i++)
        {
            for (j = 0; j < filters.Count; j++)
            {
                if (j == 0)
                {
                    continue;
                }

                if (!filters[j].Any(sp => sp.soccerPlayerId == filters[0][i].soccerPlayerId))
                {
                    break;
                }
            }

            if (j == filters.Count)
            {
                filteredList.Add(filters[0][i]);
            }
        }

        return filteredList;
    }

    public static void sortSoccerPlayers(Trend trend, TrendOf trendOf, ref List<SoccerPlayer> soccerPlayers)
    {
        if (trendOf == TrendOf.PRICE)
        {
            if (trend == Trend.ASCENDING)
            {
                soccerPlayers.Sort((x, y) => x.price.CompareTo(y.price));
                return;
            }

            soccerPlayers.Sort((x, y) => y.price.CompareTo(x.price));
            return;
        }

        if (trend == Trend.ASCENDING)
        {
            soccerPlayers.Sort((x, y) => x.score.CompareTo(y.score));
            return;
        }

        soccerPlayers.Sort((x, y) => y.score.CompareTo(x.score));
    }

    public static List<SoccerPlayer> getAllSoccerPlayers()
    {
        return SoccerPlayerConversion.ConvertDbSoccerPlayersListToSoccerPlayersList(SoccerPlayerDataMethods
            .getAllDbSoccerPlayers());
    }

    public static void sortUserSoccerPlayers(List<SoccerPlayer> soccerPlayers)
    {
        soccerPlayers.Sort((x, y) => x.role.CompareTo(y.role));
        for (int i = 0; i < soccerPlayers.Count; i++)
        {
            for (int j = i + 1; j < soccerPlayers.Count; j++)
            {
                if (soccerPlayers[i].role == soccerPlayers[j].role &&
                    soccerPlayers[i].position > soccerPlayers[j].position)
                {
                    SoccerPlayer temp = soccerPlayers[i];
                    soccerPlayers[i] = soccerPlayers[j];
                    soccerPlayers[j] = temp;
                }
            }
        }
    }

    public static List<SoccerPlayer> getUserSoccerPlayers(User user)
    {
        List<SoccerPlayer> userSoccerPlayers = new List<SoccerPlayer>();
        List<Pair<DbSoccerPlayer, Role>> userDbSoccerPlayers =
            SoccerPlayerDataMethods.getUserSoccerPlayers(UserConversion.convertUserToDbUser(user));
        foreach (var dbSoccerPlayer in userDbSoccerPlayers)
        {
            SoccerPlayer soccerPlayer =
                SoccerPlayerConversion.ConvertDbSoccerPlayerToSoccerPlayer(dbSoccerPlayer.first, dbSoccerPlayer.second);
            userSoccerPlayers.Add(soccerPlayer);
        }

        sortUserSoccerPlayers(userSoccerPlayers);
        return userSoccerPlayers;
    }
}