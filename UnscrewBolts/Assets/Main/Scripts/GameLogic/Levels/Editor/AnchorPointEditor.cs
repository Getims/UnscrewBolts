using System.Collections.Generic;
using Scripts.GameLogic.Levels.Anchors;
using Scripts.GameLogic.Levels.GameElements;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Scripts.GameLogic.Levels.Editor
{
    [CustomEditor(typeof(AnchorPoint))]
    public class AnchorPointEditor : OdinEditor
    {
        private const string ICON_PATH = "Assets/Main/Assets/Sprites/Editor/Magnet.png";
        private const float BUTTON_SIZE = 0.5f;
        private const float PICK_SIZE = 0.6f;

        private List<Transform> _holes = new List<Transform>();
        private bool _isSelectMode;

        private void OnSceneGUI()
        {
            AnchorPoint anchorPointScript = (AnchorPoint) target;
            if (anchorPointScript.IsMagnetMod)
            {
                FindAllHoles();
                anchorPointScript.IsMagnetMod = false;
            }
            
            if (!_isSelectMode)
                return;


            for (int i = 0; i < _holes.Count; i++)
            {
                Vector3 buttonPos = _holes[i].position;
                float size = HandleUtility.GetHandleSize(buttonPos) * BUTTON_SIZE;

                if (Handles.Button(buttonPos, Quaternion.identity, size, size * PICK_SIZE, Handles.SphereHandleCap))
                {
                    MoveAnchorToPoint(anchorPointScript, _holes[i]);
                    _holes.Clear();
                    _isSelectMode = false;
                }

                Texture2D magnetIcon = AssetDatabase.LoadAssetAtPath<Texture2D>($"{ICON_PATH}");
                GUIContent magnetButtonContent = new GUIContent(magnetIcon);
                GUIStyle centeredStyle = new GUIStyle();
                centeredStyle.alignment = TextAnchor.MiddleCenter;
                Handles.Label(buttonPos, magnetButtonContent, centeredStyle);
            }
        }

        private void FindAllHoles()
        {
            GameObject selectedPrefab = Selection.activeGameObject.transform.root.gameObject;
            GameElementHole[] allHoles = selectedPrefab.GetComponentsInChildren<GameElementHole>(true);
            _holes = new List<Transform>();

            foreach (GameElementHole hole in allHoles)
                _holes.Add(hole.transform);

            _isSelectMode = _holes.Count > 0;
            if (_isSelectMode)
                SceneView.RepaintAll();
        }

        private void MoveAnchorToPoint(AnchorPoint anchorPointScript, Transform hole)
        {
            Undo.RecordObject(anchorPointScript.transform, "Move Anchor Point");
            anchorPointScript.transform.position = hole.position;
            EditorUtility.SetDirty(anchorPointScript);
        }
    }
}