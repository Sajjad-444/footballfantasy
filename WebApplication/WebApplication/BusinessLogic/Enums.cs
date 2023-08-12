namespace WebApplication.BusinessLogic;

public enum UsernameEmailExistence
{
    UsernameAndEmailInMain,
    UsernameInMain,
    EmailInMain,
    UsernameAndEmailInTemp,
    UsernameInTemp,
    EmailInTemp,
    NotExistence
}

public enum Role
{
    NOTCHOSEN,
    MAIN,
    SUBSTITUTE
}

public enum Position
{
    NOTCHOSEN,
    FORWARDS,
    MIDFIELDER,
    DEFENDERS,
    GOALKEEPER
}

public enum Trend
{
    NOTCHOSEN,
    DESCENDING,
    ASCENDING,
}

public enum TrendOf
{
    NOTCHOSEN,
    SCORE,
    PRICE,
}