using System.Collections.Generic;

namespace Scripts.Data.Core
{
    public interface IDatabase
    {
        void Initialize();
        void ReloadData();
        void SaveData();
        void DeleteData();
        IEnumerable<GameData> GetAllData();
        T GetData<T>() where T : GameData;
    }
}