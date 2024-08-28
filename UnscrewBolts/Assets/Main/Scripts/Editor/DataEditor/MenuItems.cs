using UnityEditor;
using UnityEngine;

namespace Scripts.Editor.DataEditor
{
    [InitializeOnLoad]
    public class MenuItems
    {
        public const string MENU_PATH = "Tools/💾 Data Editor/";

        [MenuItem(MENU_PATH + "Clear Data")]
        public static void ClearData()
        {
            PlayerPrefs.DeleteAll();
            FileUtil.DeleteFileOrDirectory(Application.persistentDataPath);
            PlayerPrefs.Save();
        }

        [MenuItem(MENU_PATH + "Open Editor")]
        private static void OpenWindow() =>
            DataEditor.OpenEditorWindow();
    }
}