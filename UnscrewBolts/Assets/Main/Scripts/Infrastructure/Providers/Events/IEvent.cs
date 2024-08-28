namespace Scripts.Infrastructure.Providers.Events
{
    public interface IEvent
    {
        int ListenersCount();
    }
}