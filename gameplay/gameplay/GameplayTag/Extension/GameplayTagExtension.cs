using GameFramework.Entity;

namespace Game.Gameplay;

public static partial class GameplayTagExtension
{
    public static bool HasTag(this IEntity entity, string tag)
    {
        if (entity.LogicInterface is IGameEntityLogic gameEntityLogic)
            return gameEntityLogic.HasTag(tag);
        return false;
    }
    
    public static void AddTag(this IEntity entity, string tag)
    {
        if (entity.LogicInterface is IGameEntityLogic gameEntityLogic)
             gameEntityLogic.AddTag(tag);
        
    }

    public static void RemoveTag(this IEntity entity,string tag)
    {
        if (entity.LogicInterface is IGameEntityLogic gameEntityLogic)
            gameEntityLogic.RemoveTag(tag);
    }

    public static void ClearAllTag(this IEntity entity)
    {
        if (entity.LogicInterface is IGameEntityLogic gameEntityLogic)
            gameEntityLogic.ClearAllTag();
    }
}