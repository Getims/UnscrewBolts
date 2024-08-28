using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Scripts.Configs.Core;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Scripts.Editor.ConfigsEditor.ScriptableObject
{
    public static class ScriptableObjectCreator
    {
        public static void ShowDialog<T>(T obj, string defaultDestinationPath,
            Action<T> onScritpableObjectCreated = null, bool showDropdown = true)
            where T : UnityEngine.ScriptableObject
        {
            var selector = new ScriptableObjectSelector<T>(defaultDestinationPath, onScritpableObjectCreated);

            if (selector.SelectionTree.EnumerateTree().Count() == 1)
            {
                // If there is only one scriptable object to choose from in the selector, then 
                // we'll automatically select it and confirm the selection. 
                selector.SelectionTree.EnumerateTree().First().Select();
                selector.SelectionTree.Selection.ConfirmSelection();
            }
            else
            {
                // Else, we'll open up the selector in a popup and let the user choose.
                if (showDropdown)
                    selector.ShowInPopup(200);
                else
                {
                    selector.SetSelection(obj.GetType());
                    selector.SelectionTree.Selection.ConfirmSelection();
                }
            }
        }

        public static void Duplicate<T>(T asset, string newPath = null, Action<T> onScritpableObjectCreated = null)
            where T : UnityEngine.ScriptableObject
        {
            string path = AssetDatabase.GetAssetPath(asset);
            if (newPath == null)
                newPath = AssetDatabase.GenerateUniqueAssetPath(path);

            AssetDatabase.CopyAsset(path, newPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            var obj = AssetDatabase.LoadAssetAtPath<T>(newPath);
            onScritpableObjectCreated.Invoke(obj);
        }

        private class ScriptableObjectSelector<T> : OdinSelector<Type> where T : UnityEngine.ScriptableObject
        {
            private Action<T> onScritpableObjectCreated;
            private string defaultDestinationPath;

            public ScriptableObjectSelector(string defaultDestinationPath, Action<T> onScritpableObjectCreated = null,
                bool isCreate = true)
            {
                this.onScritpableObjectCreated = onScritpableObjectCreated;
                this.defaultDestinationPath = defaultDestinationPath;
                this.SelectionConfirmed += this.ShowSaveFileDialog;
            }

            protected override void BuildSelectionTree(OdinMenuTree tree)
            {
                IEnumerable<Type> scriptableObjectTypes = AssemblyUtilities
                    .GetTypes(AssemblyCategory.All)
                    .Where(x => x.IsClass && !x.IsAbstract && x.InheritsFrom(typeof(T)));

                tree.Selection.SupportsMultiSelect = false;
                tree.Config.DrawSearchToolbar = true;
                tree.AddRange(scriptableObjectTypes, x =>
                    {
                        string niceName = x.GetNiceName();
                        niceName = niceName.Replace("Config", string.Empty);

                        object obj = UnityEngine.ScriptableObject.CreateInstance(x);
                        string menuName = (obj as ScriptableConfig)?.GetConfigCategory();

                        return $"{menuName}/{niceName}";
                    })
                    .AddThumbnailIcons();
            }

            private void ShowSaveFileDialog(IEnumerable<Type> selection)
            {
                var obj = UnityEngine.ScriptableObject.CreateInstance(selection.FirstOrDefault()) as T;

                string dest = this.defaultDestinationPath.TrimEnd('/');

                if (!Directory.Exists(dest))
                {
                    Directory.CreateDirectory(dest);
                    AssetDatabase.Refresh();
                }

                dest = EditorUtility.SaveFilePanel("Save object as", dest, "New " + typeof(T).GetNiceName(), "asset");

                if (!string.IsNullOrEmpty(dest) &&
                    PathUtilities.TryMakeRelative(Path.GetDirectoryName(Application.dataPath), dest, out dest))
                {
                    AssetDatabase.CreateAsset(obj, dest);
                    AssetDatabase.Refresh();

                    if (this.onScritpableObjectCreated != null)
                    {
                        this.onScritpableObjectCreated(obj);
                    }
                }
                else
                {
                    Object.DestroyImmediate(obj);
                }
            }
        }
    }
}