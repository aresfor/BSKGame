using Game.Math;

namespace Game.Gameplay;

public static class InputUtils
{
    public static IInputUtility s_InputUtility;
    
    public static int MouseRayTraceLayer = PhysicsLayerDefine.GetMultiFlag(PhysicTraceType.StaticActor, PhysicTraceType.Pawn);
    public static void Initialize(IInputUtility inputUtility)
    {
        s_InputUtility = inputUtility;
        s_InputUtility.Init();
    }

    public static void SetMouseRayTraceLayer(int traceLayer)
    {
        MouseRayTraceLayer = traceLayer;
    }

    public static void Update(float deltaTime)
    {
        s_InputUtility.Update(deltaTime);
    }

    public static float2 GetMoveInputParam(EInputParam param)
    {
        return s_InputUtility.GetMoveInputParam(param);
    }

    public static bool GetInput(EInputParam moveParam)
    {
        return s_InputUtility.GetInput(moveParam);

    }

    public static bool GetKeyDown(EKeyCode keyCode)
    {
        return s_InputUtility.GetKeyDown(keyCode);

    }

    public static bool GetKeyUp(EKeyCode keyCode)
    {
        return s_InputUtility.GetKeyUp(keyCode);

    }

    public static bool GetKey(EKeyCode keyCode)
    {
        return s_InputUtility.GetKey(keyCode);

    }
}