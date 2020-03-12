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

        public static T? ToEnum<T>(this string value)
            where T : struct
        {
            if(!Enum.TryParse(value, out T result))
                return null;
            return result;
        }
    }
}
