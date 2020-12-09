namespace ProjectFukalite.Interfaces
{
    public interface ITrigger
    {
        bool isTriggered { get; set; }
        void Trigger();
        void UnTrigger();
    }
}