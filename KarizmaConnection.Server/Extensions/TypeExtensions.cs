namespace KarizmaPlatform.Connection.Server.Extensions;

internal static class TypeExtensions
{
    internal static bool IsGenericTaskType(this Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>);
    }
}