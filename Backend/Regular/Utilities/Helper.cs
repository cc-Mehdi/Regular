using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Utilities
{
    public static class Helper
    {
        public static int GenerateUniqueNumber(int digits = 6)
        {
            HashSet<int> existingNumbers = new HashSet<int>();

            if (digits <= 0 || digits > 9)
            {
                throw new ArgumentException("Number of digits must be between 1 and 9.");
            }

            Random random = new Random();
            int min = (int)Math.Pow(10, digits - 1);
            int max = (int)Math.Pow(10, digits) - 1;
            int newNumber;

            do
            {
                newNumber = random.Next(min, max + 1); // Generates a number between min and max (inclusive)
            } while (existingNumbers.Contains(newNumber));

            existingNumbers.Add(newNumber);
            return newNumber;
        }
        public static void SetCookie(HttpContext httpContext, string key, string value, int? expireTime = 30)
        {
            CookieOptions option = new CookieOptions();

            option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            //option.HttpOnly = true;
            //option.Secure = true;
            //option.SameSite = SameSiteMode.Strict;

            httpContext.Response.Cookies.Append(key, value, option);
        }
        public static string GetCookie(HttpContext httpContext, string key)
        {
            return httpContext.Request.Cookies[key];
        }
        public static void SetSession<T>(HttpContext httpContext, string key, T value)
        {
            var stringValue = JsonConvert.SerializeObject(value);
            httpContext.Session.SetString(key, stringValue);
        }
        public static T GetSession<T>(HttpContext httpContext, string key)
        {
            var stringValue = httpContext.Session.GetString(key);

            return stringValue == null ? default : JsonConvert.DeserializeObject<T>(stringValue);
        }
    }
}
