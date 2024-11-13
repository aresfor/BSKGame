namespace Game.UIFramework
{
    /// <summary>
    /// Base interface for all the screen properties
    /// </summary>
    public interface IScreenProperties {
        bool AlwaysPlayAnim { get; set; }
        IParameterData Data { get; set; }
    }

    public interface IParameterData { }

    /// <summary>
    /// Base interface for all Panel properties
    /// </summary>
    public interface IPanelProperties : IScreenProperties
    {
        PanelPriority Priority { get; set; }
    }

    /// <summary>
    /// Base interface for Window properties.
    /// </summary>
    public interface IWindowProperties : IScreenProperties
    {
        WindowPriority WindowQueuePriority { get; set; }
        bool HideOnForegroundLost { get; set; }
        bool IsPopup { get; set; }
        bool SuppressPrefabProperties { get; set; }
    }
}
