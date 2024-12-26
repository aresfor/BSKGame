using System;
using System.Collections.Generic;
using Game.Core;

namespace Game.Gameplay
{

    public static class GlobalUtility
    {
        private static Dictionary<Type, IUtility> s_GlobalUtility = new Dictionary<Type, IUtility>();

        public static T Get<T>() where T : IUtility
        {
            if (s_GlobalUtility.TryGetValue(typeof(T), out var utility))
            {
                return (T)utility;
            }

            throw new Exception($"try to get a not registered utility: {typeof(T)}");
        }

        public static void Register<T>(T utility) where T : IUtility
        {
            s_GlobalUtility[typeof(T)] = utility;
        }

        public static void Clear()
        {
            s_GlobalUtility.Clear();
        }
    }
}