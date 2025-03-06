using System.Globalization;
using System.Security.Claims;

namespace API.Extensions;

public static class ClaimsPrincipleExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new ArgumentException("Cannot get the userId from token"), CultureInfo.InvariantCulture);

        return userId;
    }
    public static string GetUsername(this ClaimsPrincipal user)
    {
        var username = user.FindFirstValue(ClaimTypes.Name)?? throw new ArgumentNullException("No user found in token");

        return username;
    }
}