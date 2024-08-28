using DG.Tweening;
using Scripts.UI.Base;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.UI.Tutorial
{
    public class TutorialHand : UIPanel
    {
        [SerializeField]
        private float _startY;

        [SerializeField]
        private float _targetY;

        [SerializeField, Min(0)]
        private float _moveUpTime;

        [SerializeField, Min(0)]
        private float _moveDownTime;

        [SerializeField]
        private Ease _moveUpEase;

        [SerializeField]
        private Ease _moveDownEase;

        [SerializeField]
        private RectTransform _targetRT;

        private Tweener _moveTW;

        [Button, HideInEditorMode]
        public void ShowHand(Vector2 boltPosition)
        {
            base.Show();

            MoveTo(boltPosition);
            StopAnimation();
            Movement(true);
        }

        public override void Hide()
        {
            base.Hide();
            StopAnimation();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _moveTW?.Kill();
        }

        [Button, HideInEditorMode]
        private void StopAnimation()
        {
            _moveTW.Kill();
            _moveTW = _targetRT.DOAnchorPosY(_startY, 0);
        }

        private void Movement(bool up)
        {
            float y = up ? _targetY : _startY;
            float duration = up ? _moveUpTime : _moveDownTime;
            Ease ease = up ? _moveUpEase : _moveDownEase;

            _moveTW = _targetRT.DOAnchorPosY(y, duration)
                .SetEase(ease)
                .OnComplete(() => Movement(!up));
        }

        private void MoveTo(Vector2 boltPosition)
        {
            Vector2 position = ConvertWorldPointToScreen(boltPosition);
            transform.position = position;
        }

        private Vector2 ConvertWorldPointToScreen(Vector2 point) =>
            Camera.main.WorldToScreenPoint(point);
    }
}