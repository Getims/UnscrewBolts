using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.MainMenu.Levels
{
    [Serializable]
    public class LevelsScroller
    {
        [SerializeField]
        private int _startingPanel = 0;

        [SerializeField]
        private float _scrollTime = 1f;

        [SerializeField]
        private ScrollRect _scrollRect;

        private Tweener _scrollTW;
        private RectTransform[] _panels;

        private RectTransform Content => _scrollRect.content;
        private RectTransform Viewport => _scrollRect.viewport;
        private int NumberOfPanels => Content.childCount;

        public float ScrollTime => _scrollTime;

        public void Initialize()
        {
            if (NumberOfPanels == 0) return;

            _panels = new RectTransform[NumberOfPanels];
            for (int i = 0; i < NumberOfPanels; i++)
                _panels[i] = Content.GetChild(i) as RectTransform;

            GoToPanel(_startingPanel, true);
        }

        public void SetState(bool isActive) =>
            _scrollRect.gameObject.SetActive(isActive);

        public void ScrollToLevel(int index, bool instant = false) =>
            GoToPanel(index, instant);

        public void OnDestroy() =>
            _scrollTW?.Kill();

        private void GoToPanel(int panelNumber, bool instant = false)
        {
            _scrollRect.inertia = false;

            if (instant)
                Canvas.ForceUpdateCanvases();

            float yOffset = Viewport.rect.height / 2f;
            Vector2 targetPosition = _panels[panelNumber].localPosition;
            targetPosition.y -= yOffset;

            _scrollTW?.Kill();

            if (instant)
                Content.anchoredPosition = -targetPosition;
            else
                _scrollTW = Content.DOAnchorPosY(-targetPosition.y, _scrollTime)
                    .SetEase(Ease.OutSine)
                    .OnComplete(() => _scrollRect.inertia = true);
        }
    }
}