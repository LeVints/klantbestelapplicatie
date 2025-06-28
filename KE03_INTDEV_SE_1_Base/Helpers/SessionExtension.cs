using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace KE03_INTDEV_SE_1_Base.Helpers
{
    public static class SessionExtensions
    {
        // Slaat een object op in de sessie onder de sleutel
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // Haalt een object op uit de sessie en deserializeert het naar het opgegeven type
        public static T? GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
