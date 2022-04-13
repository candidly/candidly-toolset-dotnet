using System;
using System.Runtime.CompilerServices;

namespace Candidly.Util
{
    /// <summary>
    /// It would be great to call this type <c>Convert</c>, but that name is already taken by the framework. :)
    /// </summary>
    internal static class Converter
    {
        #region Cast Utils

        public static bool TryCast<TSrc, TDest>(this TSrc value, out TDest castValue)
        {
            if (value == null && !typeof(TDest).IsValueType)
            {
                castValue = default(TDest);
                return true;
            }

            if (value is TDest castVal)
            {
                castValue = castVal;
                return true;
            }

            castValue = default(TDest);
            return false;
        }

        public static TDest Cast<TSrc, TDest>(this TSrc value)
        {
            if (TryCast<TSrc, TDest>(value, out TDest castValue))
            {
                return castValue;
            }

            throw new InvalidCastException($"Cannot cast value of type \"{typeof(TSrc).FullName}\" to type \"{typeof(TDest).FullName}\".");
        }

        public static Type TypeOf<T>(this T value)
        {
            return (value == null) ? typeof(T) : value.GetType();
        }

        #endregion Cast Utils


        #region UnixTimeSeconds Utils

        public static class UnixTimeSeconds
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static DateTimeOffset ToDateTimeOffset(long unixTimeUtcSeconds)
            {
#if NET46_OR_GREATER || NETCOREAPP
                return DateTimeOffset.FromUnixTimeSeconds(unixTimeUtcSeconds);
#else
                return ToDateTimeOffsetImplementation(unixTimeUtcSeconds);
#endif
            }

#pragma warning disable IDE0079  // Remove unnecessary suppression, as it is necessary for some targets
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0051: Remove unused private member", Justification = "Used in some, but not all compilation targets")]
#pragma warning restore IDE0079  // Remove unnecessary suppression
            private static DateTimeOffset ToDateTimeOffsetImplementation(long unixTimeUtcSeconds)
            {
                // Precomputed:
                const int DaysTo1970 = 719162;
                const long UnixEpochTicks = TimeSpan.TicksPerDay * DaysTo1970;  // 621,355,968,000,000,000
                const long MinSeconds = -62135596800;
                const long MaxSeconds = 253402300799;

                if (unixTimeUtcSeconds < MinSeconds || unixTimeUtcSeconds > MaxSeconds)
                {
                    throw new ArgumentOutOfRangeException(nameof(unixTimeUtcSeconds),
                                                          $"Valid values are between {MinSeconds} and {MaxSeconds} seconds, inclusive.");
                }

                long ticks = unixTimeUtcSeconds * TimeSpan.TicksPerSecond + UnixEpochTicks;
                return new DateTimeOffset(ticks, TimeSpan.Zero);
            }
        }

        #endregion UnixTimeSeconds Utils
    }
}
