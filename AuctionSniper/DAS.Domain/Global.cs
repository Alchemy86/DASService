using System;
namespace DAS.Domain
{
    public static class Global
    {
        public static DateTime GetPacificTime
        {
            get
            {
                var tzi = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                var localDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzi);

                return localDateTime;
            }
        }
    }
}
