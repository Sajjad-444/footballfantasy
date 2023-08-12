namespace WebApplication.Presentation;
using WebApplication.BusinessLogic;
public class messageInfo
{
    public string message { get; set; }
}

public static class StatusCode
{
    // 2xx Success
    public const int Ok = 200;
    public const int Created = 201;
    public const int Accepted = 202;
    public const int NoContent = 204;

    // 3xx Redirection
    public const int MultipleChoices = 300;
    public const int MovedPermanently = 301;
    public const int Found = 302;
    public const int SeeOther = 303;
    public const int NotModified = 304;
    public const int TemporaryRedirect = 307;
    public const int PermanentRedirect = 308;

    // 4xx Client Errors
    public const int BadRequest = 400;
    public const int Unauthorized = 401;
    public const int PaymentRequired = 402;
    public const int Forbidden = 403;
    public const int NotFound = 404;
    public const int MethodNotAllowed = 405;
    public const int NotAcceptable = 406;
    public const int ProxyAuthenticationRequired = 407;
    public const int RequestTimeout = 408;
    public const int Conflict = 409;
    public const int Gone = 410;
    public const int LengthRequired = 411;
    public const int PreconditionFailed = 412;
    public const int PayloadTooLarge = 413;
    public const int UriTooLong = 414;
    public const int UnsupportedMediaType = 415;
    public const int RequestedRangeNotSatisfiable = 416;
    public const int ExpectationFailed = 417;
    public const int UpgradeRequired = 426;
    public const int PreconditionRequired = 428;
    public const int TooManyRequests = 429;
    public const int RequestHeaderFieldsTooLarge = 431;
    public const int UnavailableForLegalReasons = 451;

    // 5xx Server Errors
    public const int InternalServerError = 500;
    public const int NotImplemented = 501;
    public const int BadGateway = 502;
    public const int ServiceUnavailable = 503;
    public const int GatewayTimeout = 504;
    public const int HttpVersionNotSupported = 505;
    public const int VariantAlsoNegotiates = 506;
    public const int InsufficientStorage = 507;
    public const int LoopDetected = 508;
    public const int NotExtended = 510;
    public const int NetworkAuthenticationRequired = 511;
}

public class ApiResponse
{
    public bool success { get; set; }
    public List<messageInfo> data { get; set; }
    public int statusCode { get; set; }
}

public class GetSoccerPlayersApiResponse : ApiResponse
{
    public List<SoccerPlayer> soccerPlayers { get; set; }
    public int pageCount { get; set; }
}

public class GetUsersApiResponse : ApiResponse
{
    public List<User> users { get; set; }
    public int pageCount { get; set; }
}