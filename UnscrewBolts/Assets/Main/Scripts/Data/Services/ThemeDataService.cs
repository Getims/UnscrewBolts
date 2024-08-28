using System.Collections.ObjectModel;
using ModestTree;
using Scripts.Data.Core;

namespace Scripts.Data.Services
{
    public interface IThemeDataService
    {
        string CurrentThemeID { get; }
        ReadOnlyCollection<string> UnlockedThemesID { get; }
        void SetCurrentTheme(string themeId, bool autosave = true);
        void UnlockTheme(string themeId, bool autosave = true);
        bool IsThemeUnlocked(string themeId);
    }

    public class ThemeDataService : DataService, IThemeDataService
    {
        private readonly ThemeData _themeData;

        public string CurrentThemeID => _themeData.CurrentThemeID;
        public ReadOnlyCollection<string> UnlockedThemesID => _themeData.UnlockedThemesID.AsReadOnly();

        public ThemeDataService(IDatabase database) : base(database) => 
            _themeData = database.GetData<ThemeData>();

        public void SetCurrentTheme(string themeId, bool autosave = true)
        {
            if (_themeData.CurrentThemeID.Equals(themeId))
                return;
            
            _themeData.CurrentThemeID = themeId;
            TryToSave(autosave);
        }

        public void UnlockTheme(string themeId, bool autosave = true)
        {
            if (_themeData.UnlockedThemesID.Contains(themeId) || themeId.IsEmpty())
                return;

            _themeData.UnlockedThemesID.Add(themeId);
            TryToSave(autosave);
        }

        public bool IsThemeUnlocked(string themeId) => 
            _themeData.UnlockedThemesID.Contains(themeId);
    }
}