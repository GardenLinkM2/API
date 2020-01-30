using System;

namespace Union.Backend.API
{
    public static class Extensions
    {
        public static string ConvertToString(this Exception ex)
        {
            return $"An error occured\n" +
                $"EXCEPTION TYPE: {ex?.GetType().Name ?? "No exception"}\n" +
                $"MESSAGE: {ex?.Message ?? "No exception message"}\n"
                #if DEBUG
                    + $"{ex?.StackTrace ?? "No exception stacktrace"}"
                #endif
                    ;
        }
    }
}
