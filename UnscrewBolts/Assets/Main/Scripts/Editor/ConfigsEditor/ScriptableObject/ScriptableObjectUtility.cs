using System.Collections.Generic;
using System.IO;
using Scripts.Configs.Core;
using UnityEditor;
using UnityEngine;

namespace Scripts.Editor.ConfigsEditor.ScriptableObject
{
    public static class ScriptableObjectUtility
    {
        public static void CreateAsset<T>() where T : UnityEngine.ScriptableObject
        {
            CreateAsset<T>(null);
        }

        public static T CreateAsset<T>(string filePath) where T : UnityEngine.ScriptableObject
        {
            T asset = UnityEngine.ScriptableObject.CreateInstance<T>();

            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (filePath == null || filePath.Length <= 0)
            {
                filePath = "Assets/" + typeof(T).ToString() + ".asset";
            }

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(filePath);

            if (assetPathAndName != filePath)
            {
                Debug.LogWarning("Asset file has been changed from " + filePath + " to " + assetPathAndName);
            }

            SaveAsset(asset, assetPathAndName);

            return asset;
        }

        public static void RenameAsset<T>(T asset, string newName) where T : UnityEngine.ScriptableObject
        {
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(asset), newName);
        }

        public static void SaveAsset<T>(T asset, string path) where T : UnityEngine.ScriptableObject
        {
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void SaveAsset<T>(T asset) where T : ScriptableConfig
        {
            EditorUtility.SetDirty(asset);

            if (GetAssetNameWithoutExtension(asset) != asset.FileName)
                RenameAsset(asset, asset.FileName);

            AssetDatabase.SaveAssets();
        }

        public static T DuplicateAsset<T>(T asset, string newPath = null) where T : UnityEngine.ScriptableObject
        {
            string path = AssetDatabase.GetAssetPath(asset);
            if (newPath == null)
                newPath = AssetDatabase.GenerateUniqueAssetPath(path);

            AssetDatabase.CopyAsset(path, newPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return AssetDatabase.LoadAssetAtPath<T>(newPath);
        }

        public static void ReplaceAsset<T>(T asset, string path) where T : UnityEngine.ScriptableObject
        {
            T oldAsset = AssetDatabase.LoadAssetAtPath<T>(path);

            if (oldAsset == null)
            {
                SaveAsset<T>(asset, path);
            }
            else
            {
                EditorUtility.CopySerialized(asset, oldAsset);
                EditorUtility.SetDirty(oldAsset);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        public static void DeleteAsset<T>(T asset) where T : UnityEngine.ScriptableObject
        {
            string path = AssetDatabase.GetAssetPath(asset);

            AssetDatabase.DeleteAsset(path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void ReloadAsset<T>(ref T asset) where T : UnityEngine.ScriptableObject
        {
            string path = AssetDatabase.GetAssetPath(asset);

            Resources.UnloadAsset(asset);
            asset = AssetDatabase.LoadAssetAtPath(path, asset.GetType()) as T;
        }

        //**** Queries ****//
        public static bool AssetExists<T>(string path) where T : UnityEngine.ScriptableObject
        {
            Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(T));

            return obj != null;
        }

        public static string GetAssetNameWithoutExtension<T>(T asset) where T : UnityEngine.ScriptableObject
        {
            string path = AssetDatabase.GetAssetPath(asset);

            return Path.GetFileNameWithoutExtension(path);
        }

        public static string GetAssetName<T>(T asset) where T : UnityEngine.ScriptableObject
        {
            string path = AssetDatabase.GetAssetPath(asset);

            return Path.GetFileName(path);
        }

        public static List<ObjectType> GetAllScriptableObjectsOfType<ObjectType>()
            where ObjectType : UnityEngine.ScriptableObject
        {
            List<ObjectType> res = new List<ObjectType>();
            string[] guids =
                AssetDatabase.FindAssets("t:" + typeof(ObjectType).FullName, new string[] {"Assets"});

            foreach (string guid in guids)
            {
                res.Add(AssetDatabase.LoadAssetAtPath<ObjectType>(
                    AssetDatabase.GUIDToAssetPath(guid)));
            }

            return res;
        }

        public static List<string> GenerateGameDatabaseOfAllScriptableObjectsOfType<ObjectType>()
            where ObjectType : UnityEngine.ScriptableObject
        {
            List<string> res = new List<string>();
            string[] guids =
                AssetDatabase.FindAssets("t:" + typeof(ObjectType).FullName, new string[] {"Assets"});

            foreach (string guid in guids)
            {
                res.Add(AssetDatabase.GUIDToAssetPath(guid).Replace("Assets/Resources", ""));
            }

            return res;
        }
    }
}