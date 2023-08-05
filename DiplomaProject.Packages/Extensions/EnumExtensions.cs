using System.Reflection;
using DiplomaProject.Packages.Attributes;
using DiplomaProject.Packages.Exceptions;
using DiplomaProject.Packages.Models;
using Microsoft.OpenApi.Extensions;

namespace DiplomaProject.Packages.Extensions;

public static class EnumExtensions
{
    public static Tenum ToEnum<Tenum>(this int value) where Tenum : struct, IConvertible
    {
        if (Enum.IsDefined(typeof(Tenum), value))
        {
            return (Tenum)Enum.ToObject(typeof(Tenum), value);
        }
        throw new EnumParseException($"Integer {value} has no value for enum {typeof(Tenum).FullName}.");
    }
    public static Tenum? ToEnum<Tenum>(this int? value) where Tenum : struct, IConvertible
    {
        return value?.ToEnum<Tenum>();
    }
    public static Tenum ToEnum<Tenum>(this string value) where Tenum : struct, IConvertible
    {
        if (Enum.IsDefined(typeof(Tenum), value))
        {
            return Enum.TryParse<Tenum>(value, true, out var result) ? result : default;
        }
        throw new EnumParseException($"String {value} has no value for enum {typeof(Tenum).FullName}.");
    }
    public static int ToEnumValue<TEnum>(this string name) where TEnum : struct, IConvertible
    {
        if (string.IsNullOrEmpty(name) || !name.IsEnumName<TEnum>())
        {
            return default;
        }

        return typeof(TEnum).ToEnumItemList()
            .Where(x => x.DisplayText == name || x.Name == name)
            .Select(x => x.Value)
            .FirstOrDefault();
    }
    public static int ToEnumValueByShortName<TEnum>(this string shortName) where TEnum : struct, IConvertible
    {
        if (string.IsNullOrEmpty(shortName))
        {
            return default;
        }

        return typeof(TEnum).ToLookupEnumItemList()
            .Where(x => x.ShortName == shortName)
            .Select(x => x.Value)
            .FirstOrDefault();
    }
    public static string ToEnumName<TEnum>(this int value) where TEnum : struct, IConvertible
    {
        return typeof(TEnum).ToEnumItemList()
            .Where(x => x.Value == value)
            .Select(x => x.Name)
            .FirstOrDefault();
    }
    public static bool IsEnumValue<TEnum>(this string name) where TEnum : struct, IConvertible
    {
        var enumList = typeof(TEnum).ToLookupEnumItemList();
        return enumList.Any(x => x.Name == name || x.DisplayText == name);
    }
    public static bool IsEnumValue<TEnum>(this int value) where TEnum : struct, IConvertible
    {
        var enumList = typeof(TEnum).ToLookupEnumItemList();
        return enumList.Any(x => x.Value == value);
    }
    public static bool IsEnumName<TEnum>(this string name) where TEnum : struct, IConvertible
    {
        var enumList = typeof(TEnum).ToLookupEnumItemList();
        return enumList.Any(x => x.Name == name || x.DisplayText == name);
    }
    public static string GetConfigValue(this Enum enumValue)
        => enumValue.GetAttributeOfType<EnumConfigAttribute>()?.Value;
    public static int GetMinValue(this Type enumType)
    {
        if (enumType.IsEnum)
        {
            return Math.Max(1, Convert.ToInt32(Enum.GetValues(enumType).Cast<IFormattable>().Min()));
        }

        throw new EnumParseException($"{enumType} is not an enum type.");
    }
    public static int GetMaxValue(this Type enumType)
    {
        if (enumType.IsEnum)
        {
            if (enumType.GetCustomAttributes<FlagsAttribute>().Any())
            {
                return Convert.ToInt32(Enum.GetValues(enumType).Cast<int>().Sum());
            }

            return Convert.ToInt32(Enum.GetValues(enumType).Cast<IFormattable>().Max());
        }

        throw new EnumParseException($"{enumType} is not an enum type.");
    }
    public static IEnumerable<int> ToEnumValueList(this Type enumType)
    {
        return Enum.GetValues(enumType).Cast<int>();
    }
    public static IList<EnumItemModel> ToLookupEnumItemList(this Type enumType) =>
        ToEnumItemList(enumType)?.Where(x => x.Value > 0).ToList();
    public static IEnumerable<EnumItemModel> ToEnumItemList(this Type enumType)
    {
        if (enumType.IsEnum)
        {
            return Enum.GetValues(enumType)
                .Cast<object>()
                .Select(e => new EnumItemModel
                {
                    Value = ((Enum)e).GetEnumValue(),
                    RawValue = (Enum)e,
                    Name = e.ToString(),
                    DisplayText = ((Enum)e).ToDisplayName(),
                    ShortName = ((Enum)e).GetShortName(),
                    Description = ((Enum)e).GetDescription(),
                    Order = ((Enum)e).GetOrderValue(),
                });
        }

        return new List<EnumItemModel>();
    }
    public static IEnumerable<ConfigItemModel> ToConfigItemList(this Type enumType)
    {
        if (enumType.IsEnum)
        {
            return Enum.GetValues(enumType)
                .Cast<object>()
                .Select(e => new ConfigItemModel
                {
                    Id = ((Enum)e).GetEnumValue(),
                    Name = e.ToString(),
                    Value = ((Enum)e).GetConfigValue()
                });
        }

        return new List<ConfigItemModel>();
    }
    public static string ToJoinedString<TEnum>(this IEnumerable<TEnum> enums) where TEnum : Enum
    {
        if (enums.Any())
        {
            return string.Join(", ", enums.Select(x => x.ToDisplayName()));
        }

        return string.Empty;
    }
    
    
    public static int? ToNullableEnumValue<TEnum>(this string name) where TEnum : struct, IConvertible
    {
        var result = ToEnumValue<TEnum>(name);
        return result == default ? null : result;
    }
    public static int? ToNullableEnumValueByShortName<TEnum>(this string name) where TEnum : struct, IConvertible
    {
        var result = ToEnumValueByShortName<TEnum>(name);
        return result == default ? null : result;
    }
    
    public static int GetEnumValue(this Enum enumValue)
    {
        try
        {
            return Convert.ToInt32(enumValue);
        }
        catch (Exception)
        {
            return BitConverter.ToInt32(new byte[] { 0, 0, 0, Convert.ToByte(enumValue) }, 0);
        }
    }
    public static string GetShortName(this Enum enumValue)
    {
        var attribute = enumValue.GetAttributeOfType<EnumDisplayAttribute>();
        return attribute?.ShortName;
    }
    public static string GetDescription(this Enum enumValue)
    {
        var attribute = enumValue.GetAttributeOfType<EnumDisplayAttribute>();
        return attribute?.Description;
    }
    public static int GetOrderValue(this Enum enumValue)
    {
        var attribute = enumValue.GetAttributeOfType<EnumDisplayAttribute>();
        return attribute?.Order ?? default;
    }
    public static string ToDisplayName(this Enum enumValue)
    {
        var attribute = enumValue.GetAttributeOfType<EnumDisplayAttribute>();
        return attribute?.Name ?? enumValue.ToString().GenerateDisplayName();
    }

}