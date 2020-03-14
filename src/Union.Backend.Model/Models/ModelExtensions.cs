using System;
using System.Collections.Generic;
using System.Linq;

namespace Union.Backend.Model.Models
{
    public static class ModelExtensions
    {
        public static List<T> ToListIfNotEmpty<T>(this IEnumerable<T> enumerable) =>
            enumerable.Count() == 0 ? null : enumerable.ToList();
        
        public static long ToSeconds(this TimeSpan timeSpan) =>
            timeSpan.Ticks / 10000000;

        public static TimeSpan ToTimeSpan(this long seconds) =>
            new TimeSpan(seconds * 10000000);

        public static double ToTimestamp(this DateTime dateTime) =>
            dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        public static DateTime ToDateTime(this double timestamp) =>
            new DateTime(1970, 1, 1).AddSeconds(timestamp);

        public static T? ToEnum<T>(this string value)
            where T : struct
        {
            if(!Enum.TryParse(value, out T result))
                return null;
            return result;
        }

        public static int ComputeDistance(this (double longitude, double latitude) from, 
                                               (double longitude, double latitude) to)
        {
            double Alongitude = from.longitude;
            double Alatitude = from.latitude;

            double Blongitude = to.longitude;
            double Blatitude = to.latitude;

            double x = (Blongitude - Alongitude) * Math.Cos(((Alatitude + Blatitude) / 2) * (Math.PI / 180.0));
            double y = Blatitude - Alatitude;
            double z = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));

            int distance = Convert.ToInt32(Math.Floor(1.852 * 60 * z));
            return distance;
        }
    }
}
