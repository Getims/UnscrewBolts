using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Scripts.Editor
{
    public class MenuItems
    {
        private const string MENU_PATH = "Tools/";
        private const string SCENES_PATH = "Assets/Main/Scenes/";
        private const string GAME_LOADER_SCENE_PATH = SCENES_PATH + "GameLoader.unity";
        
        [MenuItem(MENU_PATH + "Run Game")]
        public static void RunGame()
        {
            if (EditorApplication.isPlaying)
                return;

            OpenScene(GAME_LOADER_SCENE_PATH);
            EditorApplication.isPlaying = true;
        }

        private static void OpenScene(string path)
        {
            if (!Application.isPlaying && EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
        }
    }
}