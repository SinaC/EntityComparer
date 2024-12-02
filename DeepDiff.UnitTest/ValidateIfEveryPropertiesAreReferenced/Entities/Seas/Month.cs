using System;
using System.Collections.Generic;
using System.Globalization;

namespace DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.Seas
{
#pragma warning disable S1210 // "Equals" and the comparison operators should be overridden when implementing "IComparable"
    public readonly struct Month : IEquatable<Month>, IComparable, IComparable<Month>
#pragma warning restore S1210 // "Equals" and the comparison operators should be overridden when implementing "IComparable"
    {
        public const int MinYear = 1900;
        public const int MaxYear = 9999;
        public const int IntRepresentationMultiplier = 100;

        public int YearPart { get; }
        public int MonthPart { get; }

        public int IntRepresentation => YearPart * IntRepresentationMultiplier + MonthPart;

        public Month(int year, int month)
        {
            if (year < MinYear)
                throw new ArgumentException($"Year cannot be lower than {MinYear}", nameof(year));
            if (year > MaxYear)
                throw new ArgumentException($"Year cannot be greater than {MaxYear}", nameof(year));
            if (month < 1)
                throw new ArgumentException("Month cannot be lower than 1", nameof(month));
            if (month > 12)
                throw new ArgumentException("Month cannot be greater than 12", nameof(month));

            YearPart = year;
            MonthPart = month;
        }

        public Month(int intRepresentation)
        {
            if (intRepresentation < 0)
                throw new ArgumentException("Int representation of month cannot be negative", nameof(intRepresentation));

            int year = Math.DivRem(intRepresentation, IntRepresentationMultiplier, out int month);

            if (year < MinYear)
                throw new ArgumentException($"Calculated year cannot be lower than {MinYear}", nameof(intRepresentation));
            if (year > MaxYear)
                throw new ArgumentException($"Calculated year cannot be greater than {MaxYear}", nameof(intRepresentation));
            if (month < 1)
                throw new ArgumentException("Calculated month cannot be lower than 1", nameof(intRepresentation));
            if (month > 12)
                throw new ArgumentException("Calculated month cannot be greater than 12", nameof(intRepresentation));

            YearPart = year;
            MonthPart = month;
        }

        public Month(Date date)
        {
            var localDateTime = date.LocalDateTime;
            int year = localDateTime.Year;
            int month = localDateTime.Month;

            if (year < MinYear)
                throw new ArgumentException($"Date.Year cannot be lower than {MinYear}", nameof(date));
            if (year > MaxYear)
                throw new ArgumentException($"Date.Year cannot be greater than {MaxYear}", nameof(date));
            if (month < 1)
                throw new ArgumentException("Date.Month cannot be lower than 1", nameof(date));
            if (month > 12)
                throw new ArgumentException("Date.Month cannot be greater than 12", nameof(date));

            YearPart = year;
            MonthPart = month;
        }

        public Month(DateTime dateTime)
        {
            var localDateTime = dateTime.ToLocalTime();
            int year = localDateTime.Year;
            int month = localDateTime.Month;

            if (year < MinYear)
                throw new ArgumentException($"DateTime.Year cannot be lower than {MinYear}", nameof(dateTime));
            if (year > MaxYear)
                throw new ArgumentException($"DateTime.Year cannot be greater than {MaxYear}", nameof(dateTime));
            if (month < 1)
                throw new ArgumentException("DateTime.Month cannot be lower than 1", nameof(dateTime));
            if (month > 12)
                throw new ArgumentException("DateTime.Month cannot be greater than 12", nameof(dateTime));

            YearPart = year;
            MonthPart = month;
        }

        public Date FirstDay => new(YearPart, MonthPart, 1);

        public Date LastDay
        {
            get
            {
                int daysInMonth = DateTime.DaysInMonth(YearPart, MonthPart);
                return new Date(YearPart, MonthPart, daysInMonth);
            }
        }

        public int DayCount
        {
            get
            {
                int daysInMonth = DateTime.DaysInMonth(YearPart, MonthPart);
                return daysInMonth;
            }
        }

        public Month AddMonths(int count)
        {
            if (count == 0)
                return this;

            int year = YearPart + count / 12;
            int month = MonthPart + count % 12;

            if (month > 12)
            {
                year += 1;
                month -= 12;
            }
            else if (month <= 0)
            {
                year -= 1;
                month += 12;
            }

            return new Month(year, month);
        }

        public string GetMonthName(CultureInfo cultureInfo)
            => cultureInfo.DateTimeFormat.GetMonthName(MonthPart);

        public bool IsDayInMonth(Date date)
            => date.CompareTo(FirstDay) >= 0 && date.CompareTo(LastDay) <= 0;

        public IEnumerable<Date> EnumerateDays()
        {
            var periodFromIncluded = FirstDay;
            var periodToIncluded = LastDay;
            for (var day = periodFromIncluded; day.CompareTo(periodToIncluded) <= 0; day = day.AddDays(1))
                yield return day;
        }

        #region IEquatable<Month>, IComparable, IComparable<Month>

        public bool Equals(Month other)
            => YearPart == other.YearPart && MonthPart == other.MonthPart;

        public override bool Equals(object? obj)
            => obj is Month month && Equals(month);

        public override int GetHashCode() // this will represent a unique representation of a month
            => (YearPart << 4) + MonthPart;

        public int CompareTo(object? obj)
            => obj switch
            {
                null => 1,
                Month d => CompareTo(d),
                _ => throw new ArgumentException($"{obj} is not a {nameof(Month)}, cannot compare."),
            };

        public int CompareTo(Month other)
            => GetHashCode().CompareTo(other.GetHashCode());

        #endregion

        public override string ToString()
            => $"{MonthPart:D2}/{YearPart:D4}";
    }
}
