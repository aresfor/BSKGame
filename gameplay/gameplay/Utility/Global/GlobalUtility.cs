namespace Game.Core;

public static class GlobalUtility
{
    private static Dictionary<Type, IUtility> mGlobalUtility = new Dictionary<Type, IUtility>();

    public static T Get<T>() where T : IUtility
    {
        if (mGlobalUtility.TryGetValue(typeof(T), out var utility))
        {
            return (T)utility;
        }

        throw new Exception($"try to get a not registered utility: {typeof(T)}");
    }

    public static void Register<T>(T utility) where T : IUtility
    {
        mGlobalUtility[typeof(T)] = utility;
    }

    public static void Clear()
    {
        mGlobalUtility.Clear();
    }
}