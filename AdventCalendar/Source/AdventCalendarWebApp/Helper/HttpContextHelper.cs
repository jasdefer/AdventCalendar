using System.Text;
using Microsoft.AspNetCore.Http;

namespace AdventCalendarWebApp.Helper;

public static class HttpContextHelper
{
    public static string GetOrCreateUserId(this HttpContext context)
    {
        var userId = context.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            userId = Guid.NewGuid().ToString();
            context.Session.SetString("UserId", userId);
        }
        return userId;
    }
}
