using System;
using System.Collections.Generic;

namespace DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.Seas
{
#pragma warning disable S1210 // "Equals" and the comparison operators should be overridden when implementing "IComparable"
    public readonly struct Date : IEquatable<Date>, IComparable, IComparable<Date>
#pragma warning restore S1210 // "Equals" and the comparison operators should be overridden when implementing "IComparable"
    {
        public int DayCountSinceBaseDate { get; }

        public DateTime LocalDateTime => DateTime.SpecifyKind(new DateTime(BaseDateTimeTicks + DayCountSinceBaseDate * TimeSpan.TicksPerDay), DateTimeKind.Local);
        public DateTime UtcDateTime => DateTime.SpecifyKind(new DateTime(BaseDateTimeTicks + DayCountSinceBaseDate * TimeSpan.TicksPerDay), DateTimeKind.Local).ToUniversalTime();

        public Date(int year, int month, int day)
            : this(new DateTime(year, month, day))
        {
        }

        public Date(DateTime dateTime)
            : this(dateTime.Kind == DateTimeKind.Local
                ? (int)((dateTime.Ticks - BaseDateTimeTicks) / TimeSpan.TicksPerDay)
                : (int)((dateTime.ToLocalTime().Ticks - BaseDateTimeTicks) / TimeSpan.TicksPerDay))
        {
        }

        public Date(int days)
        {
            DayCountSinceBaseDate = days;
        }

        public Date AddDays(int days)
        {
            return new Date(DayCountSinceBaseDate + days);
        }

        public Date AddMonths(int month)
        {
            return new Date(LocalDateTime.AddMonths(month));
        }

        public IEnumerable<DateTime> EnumerateQhs()
        {
            var dateTime = LocalDateTime;
            //
            DateTime baseDate = dateTime.ToUniversalTime();
            DateTime endDate = dateTime.AddDays(1).ToUniversalTime();
            //
            while (baseDate < endDate)
            {
                yield return baseDate;
                baseDate = baseDate.AddMinutes(15);
            }
        }

        public IEnumerable<DateTime> EnumerateHours()
        {
            var dateTime = LocalDateTime;
            //
            DateTime baseDate = dateTime.ToUniversalTime();
            DateTime endDate = dateTime.AddDays(1).ToUniversalTime();
            //
            while (baseDate < endDate)
            {
                yield return baseDate;
                baseDate = baseDate.AddHours(1);
            }
        }

        #region IEquatable<Date>, IComparable, IComparable<Month>

        public bool Equals(Date other)
        {
            return DayCountSinceBaseDate == other.DayCountSinceBaseDate;
        }

        public override bool Equals(object? obj)
        {
            return obj is Date date && Equals(date);
        }

        public override int GetHashCode()
        {
            return DayCountSinceBaseDate;
        }

        public int CompareTo(object? obj)
            => obj switch
            {
                null => 1,
                Date d => CompareTo(d),
                _ => throw new ArgumentException($"{obj} is not a {nameof(Date)}, cannot compare."),
            };

        public int CompareTo(Date other)
            => DayCountSinceBaseDate.CompareTo(other.DayCountSinceBaseDate);

        #endregion

        public static Date Parse(string s)
        {
            //dd/MM/yyyy
            if (s.Length != 10
                || s[2] != '/' || s[5] != '/')
                throw new FormatException("Input string was not in correct format");
            int day = int.Parse(s[..2]);
            int month = int.Parse(s.Substring(3, 2));
            int year = int.Parse(s[6..]);
            return new Date(year, month, day);
        }

        public override string ToString()
            => LocalDateTime.ToString("dd'/'MM'/'yyyy");

        public static Date Today => new(DateTime.Today);
        public static Date MinValue => new(0);
        public static Date MaxValue => new(9999, 1, 1);

        private const long BaseDateTimeTicks = 621355968000000000; // new DateTime(1970, 01, 01).Ticks
    }
}
