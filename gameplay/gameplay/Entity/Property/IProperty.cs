
using Game.Core;

namespace Game.Gameplay;

public interface IPropertyArr
{
    public void Initialize(int propertyId, bool triggerEvent = true);
    public IReadonlyBindableProperty<float> this[int index] { get; }
    public static int Length => (int)EPropertyDefine.Max;
    public float GetProperty(EPropertyDefine propertyDefine);
    public void SetProperty(EPropertyDefine propertyDefine, float value, bool triggerEvent = true);
    public void Reset();

}