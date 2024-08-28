using Scripts.Data.Core;

namespace Scripts.Data.SaveLoad
{
    public interface ISaveLoadService
    {
        void TrySetData<TData>(string data, ref TData tData) where TData : GameData;
        void TryLoadData<TData>(ref TData tData) where TData : GameData;
        void TrySaveData<TData>(TData tData) where TData : GameData;
        void TryDeleteData<TData>(ref TData tData) where TData : GameData;
    }
}