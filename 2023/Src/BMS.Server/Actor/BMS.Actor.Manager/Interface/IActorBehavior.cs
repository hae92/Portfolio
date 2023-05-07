namespace BMS.Actor.Manager
{
    internal interface IActorBehavior
    {
        void ReceiveMessage(object message);
        void Create(object props);
        void Start(object arg);
        void Cancel();
        void Scheduler(object arg);
    }
}
