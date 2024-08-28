#if UNITY_EDITOR
using System;
using UnityEditor;

namespace Scripts.Data.Core
{
    [InitializeOnLoad]
    public static class DataEditorMediator
    {
        private static IDatabase _database;

        public static event Action<IDatabase> OnDatabaseChanged;

        public static IDatabase Database => _database;

        public static void SetDatabase(IDatabase database)
        {
            _database = database;
            OnDatabaseChanged?.Invoke(database);
        }
    }
}
#endif