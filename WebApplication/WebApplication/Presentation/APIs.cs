namespace WebApplication.Presentation;

using BusinessLogic;

public class APIs
{
    public static void Main(string[] args)
    {
        var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                builder =>
                {
                    builder.WithOrigins("https://localhost:*")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
        var app = builder.Build();
        app.UseCors(builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });

        app.MapPost("/autoSignUp", APIsMethods.signUpByToken);
        app.MapPost("/signUp", APIsMethods.signUp);
        app.MapPost("/tokenAuthentication", APIsMethods.frontTokenAuthentication);
        app.MapPost("/login", APIsMethods.loginByCredentials);
        app.MapPost("/codeVerification", APIsMethods.otpCodeVerification);
        app.MapPost("/resendOtpCode", APIsMethods.resendOtpCode);
        app.MapPost("/addSoccerPlayer", APIsMethods.addSoccerPlayer);
        app.MapPost("/removeSoccerPlayer", APIsMethods.removeSoccerPlayer);
        app.MapPost("/swapSoccerPlayersRole", APIsMethods.swapSoccerPlayersRole);
        app.MapGet("/getUserSoccerPlayers", APIsMethods.getUserSoccerPlayers);
        app.MapGet("/filterSoccerPlayersList", APIsMethods.filterSoccerPlayers);
        app.MapPost("/sendConfirmationEmail", APIsMethods.sendConfirmationEmail);
        app.MapPost("/setNewPassword", APIsMethods.setNewPassword);
        app.MapGet("/getUsers", APIsMethods.getUsers);
        app.MapPost("/deleteAccount", APIsMethods.deleteAccount);
        app.MapGet("/getUserInfo", APIsMethods.getUserInfo);
        SoccerPlayersTableUpdateScheduler.updateSoccerPlayersTable(null, null);
        var soccerPlayersTableUpdateScheduler = new SoccerPlayersTableUpdateScheduler();
        soccerPlayersTableUpdateScheduler.Start();
        app.Run("http://localhost:3001");
    }
}