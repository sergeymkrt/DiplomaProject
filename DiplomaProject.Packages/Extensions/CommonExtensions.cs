using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using DiplomaProject.Packages.Enums;

namespace DiplomaProject.Packages.Extensions;

public static class BooleanExtensions
{
    public static string ToYesNoString(this bool value)
    {
        return value ? OptionType.Yes.ToString() : OptionType.No.ToString();
    }
}

public static class CommonExtensions
{
    public const string DateFormatGerman = "dd.MM.yyyy";
    
    public static IEnumerable<T> GetAttributes<T>(this ICustomAttributeProvider source, bool inherit) where T : Attribute
    {
        var attrs = source.GetCustomAttributes(typeof(T), inherit);

        return (attrs != null) ? (T[])attrs : Enumerable.Empty<T>();
    }
    
    public static IList ToGenericList(this IEnumerable collection, Type propertyType)
    {
        var type = propertyType.GenericTypeArguments[0];
        var listType = typeof(List<>).MakeGenericType(type);
        var list = (IList)Activator.CreateInstance(listType);

        foreach (var item in collection)
        {
            list?.Add(item);
        }

        return list;
    }
    
    public static List<MemberInfo> GetMembersFromExpression<TSource, TCondition>(
        this Expression<Func<TSource, TCondition>> expression)
    {
        var members = new List<MemberInfo>();
        if (expression != null)
        {
            if (expression.Body is NewExpression key)
            {
                foreach (var member in key.Members)
                {
                    members.Add(member);
                }
            }
            else if (expression.Body is UnaryExpression unaryExpression)
            {
                if (unaryExpression.Operand is MemberExpression memberExpression)
                {
                    members.Add(memberExpression.Member);
                }
            }
            else if (expression.Body is MemberExpression memberExpression)
            {
                members.Add(memberExpression.Member);
            }
            else if (expression.Body is MemberInitExpression memberInitExpression)
            {
                foreach (var memberBinding in memberInitExpression.Bindings)
                {
                    members.Add(memberBinding.Member);
                }
            }
        }

        return members;
    }
    
    public static string GetDisplayName(this MemberInfo member)
    {
        if (Attribute.GetCustomAttribute(member, typeof(DisplayNameAttribute)) is DisplayNameAttribute attribute)
        {
            return attribute.DisplayName;
        }

        return member.Name.GenerateDisplayName();
    }
    
    public static string GenerateDisplayName(this string propertyName)
    {
        var regex = new Regex(@"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])");
        return regex.Replace(propertyName, " ")
            .Replace(" Id", "", StringComparison.InvariantCulture)
            .Replace(" ID", "", StringComparison.InvariantCulture);
    }
    
    /// <summary>
    /// Convert string in german date format (dd.MM.yyyy) to datetime or return a parse error.
    /// </summary>
    /// <param name="germanDateString"></param>
    /// <returns>The converted German date string as DateTime type</returns>
    /// <remarks></remarks>
    public static DateTime ToDate(this string germanDateString)
    {
        var germanCultureInfo = new CultureInfo("de-de");
        return !DateTime.TryParseExact(germanDateString.Trim(), germanCultureInfo.DateTimeFormat.ShortDatePattern,
            germanCultureInfo, DateTimeStyles.None, out var result)
            ? default
            : result;
    }
    
    public static int GetQuarter(this DateTime fromDate)
    {
        int month = fromDate.Month - 1;
        int month2 = Math.Abs(month / 3) + 1;
        return month2;
    }
    
    public static HashSet<DateTime> GetSeventhDates(this DateTime startDate)
    {
        var seventDates = new HashSet<DateTime>();

        var schedule = new HashSet<DateTime>() {
            new(startDate.Year, 01, 07),
            new(startDate.Year, 04, 07),
            new(startDate.Year, 07, 07),
            new(startDate.Year, 10, 07),
            new(startDate.Year + 1, 01, 07)
        };

        var scheduleStartDate = schedule.OrderBy(d => d).First(d => d > startDate);
        seventDates.Add(scheduleStartDate);

        for (int i = 0; i < 3; i++)
        {
            seventDates.Add(scheduleStartDate.AddMonths(3));
            scheduleStartDate = scheduleStartDate.AddMonths(3);
        }

        return seventDates;
    }
    
    public static HashSet<DateTime> GetFirstFridays(this DateTime startDate)
    {
        var firstFridays = new HashSet<DateTime>();

        var firstFriday = new DateTime(startDate.Year, startDate.Month, 01);
        var lastDay = firstFriday.AddMonths(12).AddDays(-1);

        while (firstFriday <= lastDay)
        {
            if (firstFriday.DayOfWeek == DayOfWeek.Friday)
            {
                firstFridays.Add(firstFriday);
                firstFriday = new DateTime(firstFriday.Year, firstFriday.Month, 1).AddMonths(1);
            }
            else
            {
                firstFriday = firstFriday.AddDays(1);
            }
        }

        return firstFridays.Where(d => d >= startDate).ToHashSet();
    }
    
    public static HashSet<DateTime> GetQuarterEndDates(this DateTime startDate)
    {
        var quarterEndDates = new HashSet<DateTime>();

        var lastDayOfQuarter = new DateTime(startDate.Year, startDate.Month, 1).AddMonths(3 - ((startDate.Month - 1) % 3)).AddDays(-1);
        var lastQuarter = new DateTime(lastDayOfQuarter.Year, lastDayOfQuarter.Month, 1).AddMonths(10).AddDays(-1);

        while (lastDayOfQuarter <= lastQuarter)
        {
            quarterEndDates.Add(lastDayOfQuarter);
            lastDayOfQuarter = new DateTime(lastDayOfQuarter.Year, lastDayOfQuarter.Month, 1).AddMonths(4).AddDays(-1);
        }

        return quarterEndDates;
    }
    
    public static T DeepClone<T>(this T source) where T : class
    {
        var output = JsonSerializer.Serialize(source);
        return JsonSerializer.Deserialize<T>(output);
    }
    
    public static string StripInvalidFileCharacters(this string fileName)
    {
        return fileName.StripWhiteSpaces().Replace(",", "").Replace("(", "").Replace(")", "");
    }
    
    /// <summary>
    /// Remove white spaces from a string. If string is null return nothing.
    /// </summary>
    /// <param name="aString"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static string StripWhiteSpaces(this string aString)
    {
        if (aString is object)
        {
            aString = aString.Replace(" ", "");
        }

        return aString.Trim();
    }
    
    /// <summary>
    /// Returns a date Formatted in dd.MM.yyyy format, or empty string if passed date is nothing.
    /// </summary>
    /// <param name="aDate"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public static string ToGerman(this DateTime aDate)
    {
        if (aDate == default || aDate == DateTime.MinValue)
        {
            return string.Empty;
        }

        return aDate.ToString(DateFormatGerman);
    }

    public static string ToGerman(this DateTime? aDate)
    {
        if (aDate == null || aDate == DateTime.MinValue)
        {
            return string.Empty;
        }

        return DateTime.Parse(Convert.ToString(aDate)).ToString(DateFormatGerman);
    }
    /// <summary>
    /// Adds trailing zeros to the end of the number.
    /// If numberOfDecimalPlaces is smaller than the current value, no padding occurs and the original value
    /// is returned as a string with no padding.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="showZero"></param>
    /// <param name="numberOfDecimalPlaces"></param>
    /// <returns></returns>
    public static string PadTrailingZeros(this decimal value, bool showZero, int numberOfDecimalPlaces)
    {
        if (value == 0)
        {
            if (showZero)
            {
                if (numberOfDecimalPlaces == 0)
                {
                    // Only return 0 in this case, as there may be need to return 0.00
                    return "0";
                }
            }
            else
            {
                return string.Empty;
            }
        }

        int placesPastDecimal = value.NumberOfDecimalPlaces();
        return value.ToCommaSeparatedDecimals(numberOfDecimalPlaces < placesPastDecimal ? placesPastDecimal : numberOfDecimalPlaces);
    }

    public static string PadTrailingZeros(this decimal? value, bool showZero, int numberOfDecimalPlaces)
    {
        if (!value.HasValue)
        {
            return string.Empty;
        }

        return value.Value.PadTrailingZeros(showZero, numberOfDecimalPlaces);
    }

    public static int NumberOfDecimalPlaces(this decimal value)
    {
        string numberAsString = value.ToString("#,###.####################");
        int indexOfDecimalPoint = numberAsString.IndexOf(".");
        if (indexOfDecimalPoint == -1) // No decimal point in number
        {
            return 0;
        }
        else
        {
            return numberAsString.Substring(indexOfDecimalPoint + 1).Length;
        }
    }

    public static int NumberOfDecimalPlaces(this decimal? value)
    {
        return value.HasValue ? value.Value.NumberOfDecimalPlaces() : 0;
    }

    public static string ToCommaSeparatedDecimals(this decimal value, int numberOfDecimalPlaces)
    {
        var response = "{0:N" + numberOfDecimalPlaces + "}";
        return string.Format("" + response, value);
    }

    public static string AddPercentSign(this string value, bool withSpace = false)
    {
        return withSpace ? value + " %" : value + "%";
    }

    public static decimal ToDecimalOrZero(this decimal? value)
    {
        return value ?? 0;
    }
    public static object ToDBValue(this object value)
    {
        return value ?? DBNull.Value;
    }

    public static object FromDBValue(this object value)
    {
        return Convert.IsDBNull(value) ? null : value;
    }
    
}