namespace WebApplication.Presentation;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using WebApplication.BusinessLogic;

public class JwtManager
{
    private const string secretKey = "adslsdlfkjdsqweogjnasdhfo32432fwdfergwqeoqijhgas";

    private static readonly SymmetricSecurityKey securityKey =
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

    public static string generateToken(User user)
    {
        var claims = new[]
        {
            new Claim("userName", user.userName),
            new Claim("email", user.emailAddress)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    // public static void storeNewTokenInCookie(HttpContext httpContext, User user)
    // {
    //     string token = generateToken(user);
    //     CookieOptions options = new CookieOptions
    //     {
    //         HttpOnly = true,
    //         Secure = false,
    //         Path = "/",
    //         Expires = DateTime.Now.AddDays(7),
    //     };
    //     httpContext.Response.Cookies.Append("footballFantasyToken", token, options);
    // }

    public static bool verifyToken(string token)
    {
        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            var handler = new JwtSecurityTokenHandler();
            handler.ValidateToken(token, validationParameters, out var validatedToken);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static string decodeToken(string token)
    {
        if (!verifyToken(token))
        {
            return null;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        return jwtToken.Claims.First(claim => claim.Type == "userName").Value;
    }
}