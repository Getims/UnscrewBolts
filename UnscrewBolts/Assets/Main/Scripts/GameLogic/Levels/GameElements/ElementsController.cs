using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.GameLogic.Levels.GameElements
{
    public class ElementsController : MonoBehaviour
    {
        [SerializeField]
        private List<GameElement> _gameElements = new List<GameElement>();

        private Action _noElementsEvent;

        public void Initialize(Action noElementsEvent)
        {
            _noElementsEvent = noElementsEvent;

            foreach (GameElement gameElement in _gameElements)
            {
                gameElement.Initialize();
                gameElement.OnDestroyEvent += RemoveElement;
            }
        }

        public void ShowElements()
        {
            foreach (GameElement gameElement in _gameElements)
                gameElement.Show();
        }

        public void HideElements(bool instant)
        {
            foreach (GameElement gameElement in _gameElements)
                gameElement.Hide(instant);
        }

        [Button]
        public void CollectElements()
        {
#if UNITY_EDITOR
            GameElement[] gameElements = GetComponentsInChildren<GameElement>();
            _gameElements.Clear();
            _gameElements.AddRange(gameElements);

            int layer = 1;
            foreach (var gameElement in _gameElements)
            {
                gameElement.UpdateLayer(layer);
                layer++;
            }
#endif
        }

        private void OnDestroy()
        {
            foreach (GameElement gameElement in _gameElements)
            {
                if (gameElement == null)
                    continue;

                gameElement.OnDestroyEvent -= RemoveElement;
            }
        }

        private void RemoveElement(GameElement gameElement)
        {
            gameElement.OnDestroyEvent -= RemoveElement;
            _gameElements.Remove(gameElement);

            if (_gameElements.Count == 0)
                _noElementsEvent?.Invoke();
        }
    }
}