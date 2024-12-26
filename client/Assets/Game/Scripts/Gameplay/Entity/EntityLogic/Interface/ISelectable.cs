namespace Game.Client
{
    public interface ISelectable
    {
        bool Selected { get; }
        void OnSelect();
        void OnDeSelect();
    }
}