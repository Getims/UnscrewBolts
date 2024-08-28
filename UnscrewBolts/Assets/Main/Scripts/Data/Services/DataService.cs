using Scripts.Data.Core;

namespace Scripts.Data.Services
{
    public class DataService
    {
        private readonly IDatabase _database;

        protected DataService(IDatabase database) =>
            _database = database;

        protected void TryToSave(bool autoSave)
        {
            if (!autoSave)
                return;

            _database.SaveData();
        }
    }
}