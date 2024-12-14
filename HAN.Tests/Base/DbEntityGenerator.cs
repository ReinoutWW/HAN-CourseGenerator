using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace HAN.Tests.Base;

public static class DbEntityCreator<T> where T : class, new()
{
    public static T CreateEntity()
    {
        var entity = new T();
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        properties.ToList()
            .ForEach(prop => GetValueForEntity(prop, entity));

        return entity;
    }

    private static void GetValueForEntity(PropertyInfo property, T entity)
    {
        if (property.Name.Equals("Id", StringComparison.OrdinalIgnoreCase))
        {
            property.SetValue(entity, null);
            return;
        }
        
        var value = GenerateValueForProperty(property);
        property.SetValue(entity, value);
    }

    private static object? GenerateValueForProperty(PropertyInfo property)
    {
        var propertyType = property.PropertyType;

        // Handle string properties with DataAnnotations
        if (propertyType == typeof(string))
        {
            var minLength = property.GetCustomAttribute<MinLengthAttribute>()?.Length ?? 1;
            var maxLength = property.GetCustomAttribute<MaxLengthAttribute>()?.Length ?? 10;

            return GenerateString(minLength, maxLength);
        }

        // Handle integer properties with RangeAttribute
        if (propertyType == typeof(int) || propertyType == typeof(int?))
        {
            var range = property.GetCustomAttribute<RangeAttribute>();
            var minValue = range?.Minimum as int? ?? 0;
            var maxValue = range?.Maximum as int? ?? 100;

            return GenerateInt(minValue, maxValue);
        }

        // Handle GUID properties
        if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
        {
            return Guid.NewGuid();
        }

        // Handle DateTime properties
        if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
        {
            return GenerateDateTime();
        }

        // Handle Boolean properties
        if (propertyType == typeof(bool) || propertyType == typeof(bool?))
        {
            return GenerateBoolean();
        }
        
        // List, instanciate new list
        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>))
        {
            return Activator.CreateInstance(propertyType);
        }
        
        // Add more types if needed (e.g., float, double, etc.)
        return null;
    }

    private static string GenerateString(int minLength, int maxLength)
    {
        var length = new Random().Next(minLength, maxLength + 1);
        return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", length)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());
    }

    private static int GenerateInt(int minValue, int maxValue)
    {
        return new Random().Next(minValue, maxValue + 1);
    }

    private static DateTime GenerateDateTime()
    {
        return DateTime.Now.AddDays(new Random().Next(-365, 365));
    }

    private static bool GenerateBoolean()
    {
        return new Random().Next(0, 2) == 0;
    }
}