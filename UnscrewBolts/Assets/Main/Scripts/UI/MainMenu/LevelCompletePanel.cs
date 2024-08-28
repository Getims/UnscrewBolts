using DG.Tweening;
using Scripts.UI.Base;
using UnityEngine;

namespace Scripts.UI.MainMenu
{
    public class LevelCompletePanel : UIPanel
    {
        [SerializeField]
        private Transform _background;

        [SerializeField]
        private Transform _text;

        private Sequence _showSQ;
        private float ScaleTime => FadeTime;
        private float ShrinkScaleTime => FadeTime * 0.15f;

        public override void Show()
        {
            Show(0);
        }

        public override void Show(bool instant)
        {
            if (instant)
            {
                _showSQ?.Complete();
                base.Show(instant);
            }
            else
                Show(0);
        }

        public override void Show(float delay)
        {
            _background.localScale = new Vector3(0, 1, 1);
            _text.localScale = new Vector3(0, 0, 1);

            base.Show(delay);

            if (_showSQ == null)
                _showSQ = DOTween.Sequence();
            else
                _showSQ?.Kill();

            _showSQ.SetDelay(delay);
            _showSQ.Append(_background.DOScaleX(1.1f, ScaleTime));
            _showSQ.Append(_background.DOScaleX(1f, ShrinkScaleTime));
            _showSQ.Join(_text.DOScale(new Vector3(1.1f, 1.1f, 1), ScaleTime));
            _showSQ.Append(_text.DOScale(Vector3.one, ShrinkScaleTime));
            _showSQ.Play();
        }

        public float GetShowTime() => ScaleTime + ScaleTime + ShrinkScaleTime;

        public override void Hide(bool instant)
        {
            base.Hide(instant);
            if (instant)
                _showSQ?.Kill();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _showSQ?.Kill();
        }
    }
}