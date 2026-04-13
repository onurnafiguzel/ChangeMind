namespace ChangeMind.Application.Extensions;

public static class EnumExtensions
{
    public static T ParseOrThrow<T>(this string value) where T : struct, Enum
    {
        if (!Enum.TryParse<T>(value, ignoreCase: true, out var result))
            throw new ArgumentException($"Invalid value '{value}' for {typeof(T).Name}.");
        return result;
    }
}
