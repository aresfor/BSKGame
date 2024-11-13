namespace Game.UIFramework {
    /// <summary>
    /// An entry for controlling window history and queue
    /// </summary>
    public struct WindowHistoryEntry
    {
        public readonly IWindowController Screen;
        public readonly IWindowProperties Properties;
        public bool needPlayAnim;

        public WindowHistoryEntry(IWindowController screen, IWindowProperties properties, bool playAnim) {
            Screen = screen;
            Properties = properties;
            needPlayAnim = playAnim;
        }

        public void Open(bool playAnim) {
            Screen.Open(playAnim, Properties);
        }
    }
}
