using System;
using Scripts.GameLogic.Levels.Anchors;
using Scripts.GameLogic.Levels.Bolts;
using Scripts.GameLogic.Levels.Boosters;
using Scripts.GameLogic.Levels.GameElements;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.GameLogic.Levels
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField]
        private AnchorsController _anchorsController;

        [SerializeField]
        private BoltsController _boltsController;

        [SerializeField]
        private ElementsController _elementsController;

        [SerializeField]
        private BoosterController _boosterController;

        public event Action NoElementsEvent;

        public void Initialize()
        {
            _anchorsController.Initialize();
            _elementsController.Initialize(SendNoElementsEvent);
            _boltsController.Initialize(_anchorsController.GetBolts());
            _boosterController.Initialize();
        }

        public void Show()
        {
            _anchorsController.ShowAnchors();
            _boltsController.ShowBolts();
            _elementsController.ShowElements();
        }

        public void Hide(bool instant)
        {
            _anchorsController.HideAnchors(instant);
            _boltsController.HideBolts(instant);
            _elementsController.HideElements(instant);
        }

        private void SendNoElementsEvent() => NoElementsEvent?.Invoke();

        [Button]
        private void ValidateController()
        {
#if UNITY_EDITOR
            _anchorsController = GetComponentInChildren<AnchorsController>();
            if (_anchorsController == null)
                Debug.LogError("AnchorsController not found!");
            _anchorsController.CollectAnchors();

            _boltsController = GetComponentInChildren<BoltsController>();
            if (_boltsController == null)
                Debug.LogError("BoltsController not found!");

            _elementsController = GetComponentInChildren<ElementsController>();
            if (_elementsController == null)
                Debug.LogError("ElementsController not found!");
            _elementsController.CollectElements();
#endif
        }
    }
}