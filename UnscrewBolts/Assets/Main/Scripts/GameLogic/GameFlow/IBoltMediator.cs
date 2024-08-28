using Scripts.GameLogic.Levels.Bolts;

namespace Scripts.GameLogic.GameFlow
{
    public interface IBoltMediator
    {
        Bolt GetSelectedBolt();
        void SetSelectedBolt(Bolt bolt);
        void UnselectCurrentBolt();
    }
}