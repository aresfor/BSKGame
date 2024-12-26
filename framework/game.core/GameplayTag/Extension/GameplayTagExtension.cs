using GameFramework.Entity;

namespace Game.Gameplay
{

    public static partial class GameplayTagExtension
    {
        public static bool HasTag(this IEntity entity, string tag)
        {
            return entity.LogicInterface.HasTag(tag);
        }

        public static void AddTag(this IEntity entity, string tag)
        {
            entity.LogicInterface.AddTag(tag);

        }

        public static void RemoveTag(this IEntity entity, string tag)
        {
            entity.LogicInterface.RemoveTag(tag);
        }

        public static void ClearAllTag(this IEntity entity)
        {
            entity.LogicInterface.ClearAllTag();
        }
    }
}