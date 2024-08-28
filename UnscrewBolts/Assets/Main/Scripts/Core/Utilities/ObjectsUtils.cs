using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Scripts.Core.Utilities
{
    public static partial class Utils
    {
        public static string GetUniqueID() =>
            Guid.NewGuid().ToString().Remove(0, 20);

        public static string GetUniqueID(int length)
        {
            string randomString = Guid.NewGuid().ToString();
            if (length < randomString.Length)
                randomString = randomString.Substring(0, length);
            return randomString;
        }

        public static List<T> GetAllScriptableObjectsOfType<T>() where T : ScriptableObject
        {
            List<T> res = new List<T>();

#if UNITY_EDITOR
            string typeName = typeof(T).FullName;

            string[] guids = AssetDatabase.FindAssets("t:" + typeName, new string[] {"Assets"});
            foreach (string guid in guids)
                res.Add(AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid)));
#endif
            return res;
        }

        public static List<T> LoadAllPrefabsOfType<T>(string path) where T : MonoBehaviour
        {
            List<T> prefabComponents = new List<T>();
#if UNITY_EDITOR
            if (path != "")
            {
                if (path.EndsWith("/"))
                {
                    path = path.TrimEnd('/');
                }
            }

            DirectoryInfo dirInfo = new DirectoryInfo(path);
            foreach (var dir in dirInfo.GetDirectories())
            {
                prefabComponents = LoadAllPrefabsOfType<T>(dir.FullName);
            }

            FileInfo[] fileInf = dirInfo.GetFiles("*.prefab");

            //loop through directory loading the game object and checking if it has the component you want
            foreach (FileInfo fileInfo in fileInf)
            {
                string fullPath = fileInfo.FullName.Replace(@"\", "/");
                string assetPath = "Assets" + fullPath.Replace(Application.dataPath, "");
                GameObject prefab = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;

                if (prefab != null)
                {
                    T hasT = prefab.GetComponent<T>();
                    if (hasT != null)
                    {
                        prefabComponents.Add(hasT);
                    }
                }
            }
#endif
            return prefabComponents;
        }
    }
}