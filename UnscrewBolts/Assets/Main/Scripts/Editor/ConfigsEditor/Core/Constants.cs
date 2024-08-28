using UnityEngine;

namespace Scripts.Editor.ConfigsEditor.Core
{
    public static class Titles
    {
        public const string NEW_CONFIG_FILENAME = "Config";
    }

    public static class Paths
    {
        public const string ASSETS_PATH = "Assets/";
        public const string MENU_PATH = "Tools/⚙ Config Editor/";
        public const string EDITOR_CONFIG_PATH = ASSETS_PATH + "Main/Resources/Configs/ConfigEditor/";
        public const string EDITOR_CONFIG_NAME = "EditorConfig.asset";
        public const string CATEGORIES_PATH = ASSETS_PATH + "Main/Scripts/Configs/Core/ConfigsCategories.cs";
        public const string CONSTANTS_PATH = ASSETS_PATH + "Main/Scripts/Editor/ConfigsEditor/Core/Constants.cs";
    }

    public static class ConfigWindow
    {
        public const string SCRIPTS_BUTTONS = "Config sripts";
        public const float WIDTH = 430;
        public const float HEIGHT = 250;
        public const int BUTTON_HEIGHT = 30;
    }

    public static class Colors
    {
        public static readonly Color SelectedColor = new Color(0.745f, 0.256f, 0.302f);
        public static readonly Color SelectedInactiveColor = new Color(0.205f, 0.205f, 0.205f);
    }
}