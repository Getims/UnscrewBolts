using System.Collections.Generic;
using Scripts.GameLogic.Levels.Anchors;
using Scripts.GameLogic.Levels.GameElements;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Scripts.GameLogic.Levels.Editor
{
    [CustomEditor(typeof(GameElement))]
    public class GameElementInspector : OdinEditor
    {
        private const string ICON1_PATH = "Assets/Main/Assets/Sprites/Editor/MagnetElement.png";
        private const string ICON2_PATH = "Assets/Main/Assets/Sprites/Editor/Magnet.png";

        private static List<Transform> _otherHoles = new List<Transform>();
        private static bool _isMagnetMode = false;
        private static Transform _selectedHole;

        private GameElement _gameElement;

        private void OnSceneGUI()
        {
            _gameElement = target as GameElement;
            if (_gameElement == null)
                return;

            if (!_gameElement.IsEditMode)
            {
                _isMagnetMode = false;
                return;
            }

            if (!_isMagnetMode)
            {
                for (int i = 0; i < _gameElement.ElementHoles.Count; i++)
                {
                    ShowHandle(i);
                    ShowMagnet(i);
                }
            }
            else
            {
                foreach (Transform otherHole in _otherHoles)
                    ShowOtherMagnet(otherHole);
            }
        }

        private void ShowHandle(int index)
        {
            Handles.color = Color.blue;

            EditorGUI.BeginChangeCheck();
            Vector3 pointPosition = _gameElement.ElementHoles[index].transform.position;
            Quaternion rotation = _gameElement.transform.rotation;
            Quaternion newTargetRotation = Handles.RotationHandle(rotation, pointPosition);


            if (!EditorGUI.EndChangeCheck())
                return;

            Undo.RecordObject(_gameElement.transform, "Change rotation");
            var startAngle = rotation.eulerAngles;
            var newAngle = newTargetRotation.eulerAngles;
            var difference = newAngle - startAngle;

            _gameElement.transform.RotateAround(pointPosition, Vector3.forward, difference.z);
        }

        private void ShowMagnet(int index)
        {
            Vector3 buttonPos = _gameElement.ElementHoles[index].transform.position;
            float size = HandleUtility.GetHandleSize(buttonPos) * 0.35f;

            if (Handles.Button(buttonPos, Quaternion.identity, size, size, Handles.SphereHandleCap))
            {
                _selectedHole = _gameElement.ElementHoles[index].transform;
                FindOtherHoles();
            }

            Texture2D magnetIcon = AssetDatabase.LoadAssetAtPath<Texture2D>($"{ICON1_PATH}");
            GUIContent magnetButtonContent = new GUIContent(magnetIcon);
            GUIStyle centeredStyle = new GUIStyle();
            centeredStyle.alignment = TextAnchor.MiddleCenter;
            Handles.Label(buttonPos, magnetButtonContent, centeredStyle);
        }

        private void FindOtherHoles()
        {
            _otherHoles.Clear();
            GameObject selectedPrefab = Selection.activeGameObject.transform.root.gameObject;
            GameElementHole[] allHoles = selectedPrefab.GetComponentsInChildren<GameElementHole>(true);
            AnchorPoint[] allAnchors = selectedPrefab.GetComponentsInChildren<AnchorPoint>(true);
            foreach (GameElementHole hole in allHoles)
            {
                if (_gameElement.ElementHoles.Contains(hole))
                    continue;

                _otherHoles.Add(hole.transform);
            }

            foreach (AnchorPoint anchorPoint in allAnchors)
                _otherHoles.Add(anchorPoint.transform);

            _isMagnetMode = _otherHoles.Count > 0;
            if (_isMagnetMode)
                SceneView.RepaintAll();
        }

        private void ShowOtherMagnet(Transform hole)
        {
            Vector3 buttonPos = hole.position;
            float size = HandleUtility.GetHandleSize(buttonPos) * 0.35f;

            if (Handles.Button(buttonPos, Quaternion.identity, size, size, Handles.SphereHandleCap))
            {
                _isMagnetMode = false;
                Magnetize(_selectedHole, hole);
            }

            Texture2D magnetIcon = AssetDatabase.LoadAssetAtPath<Texture2D>($"{ICON2_PATH}");
            GUIContent magnetButtonContent = new GUIContent(magnetIcon);
            GUIStyle centeredStyle = new GUIStyle();
            centeredStyle.alignment = TextAnchor.MiddleCenter;
            Handles.Label(buttonPos, magnetButtonContent, centeredStyle);
        }

        private void Magnetize(Transform selectedHole, Transform targetHole)
        {
            Vector3 offset = selectedHole.position - _gameElement.transform.position;
            _gameElement.transform.position = targetHole.position - offset;
        }
    }
}