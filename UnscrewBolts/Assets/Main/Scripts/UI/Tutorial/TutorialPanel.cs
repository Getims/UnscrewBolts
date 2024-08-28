using System;
using System.Collections.Generic;
using Scripts.Data.Services;
using Scripts.GameLogic.Levels.Anchors;
using Scripts.Infrastructure.Providers.Events;
using Scripts.UI.Base;
using UnityEngine;
using Zenject;

namespace Scripts.UI.Tutorial
{
    public class TutorialPanel : UIPanel
    {
        [SerializeField]
        private TutorialText _tutorialText;

        [SerializeField]
        private TutorialHand _tutorialHand;

        [SerializeField]
        private List<TutorialStep> _steps;

        private readonly TutorialController _tutorialController = new TutorialController();
        private bool _hasTutorial;
        private LocalEventProvider _localEventProvider;

        public event Action OnTutorialCompleteEvent;

        [Inject]
        public void Construct(LocalEventProvider localEventProvider, IProgressDataService progressDataService)
        {
            _localEventProvider = localEventProvider;
            _localEventProvider.AddListener<AnchorClickEvent, AnchorClickData>(OnAnchorClick);
        }

        public override void Show()
        {
            base.Show();
            _tutorialController.StartTutorial(_steps, _tutorialHand, _tutorialText, OnTutorialComplete);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _localEventProvider.RemoveListener<AnchorClickEvent, AnchorClickData>(OnAnchorClick);
        }

        private void OnAnchorClick(AnchorClickData anchorClickData)
        {
            if (anchorClickData.SetScrewed)
                _tutorialController.OnScrewBolt(anchorClickData.AnchorPosition);
            else
                _tutorialController.OnUnscrewBolt(anchorClickData.AnchorPosition);
        }

        private void OnTutorialComplete()
        {
            _localEventProvider.RemoveListener<AnchorClickEvent, AnchorClickData>(OnAnchorClick);
            Hide();
            OnTutorialCompleteEvent?.Invoke();
        }
    }
}