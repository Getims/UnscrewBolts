namespace Scripts.Infrastructure.StateMachines.BaseStates
{
    public interface ILateTickableState
    {
        void LateTick();
    }
}