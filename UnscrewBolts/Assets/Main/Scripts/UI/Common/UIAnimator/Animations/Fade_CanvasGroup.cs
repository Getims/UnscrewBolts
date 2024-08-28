using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.UI.Common.UIAnimator.Animations
{
    [Serializable]
    public class Fade_CanvasGroup : Animation
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        [HideIf(nameof(_instant))]
        private Fade _fadeType;

        [SerializeField]
        [ShowIf(nameof(_instant))]
        private float _alphaValue;

        private Tweener _fadeTW;

        public override void Play()
        {
            if (_instant)
            {
                _canvasGroup.alpha = _alphaValue;
                return;
            }

            _fadeTW?.Kill();
            float endValue = 1;
            _canvasGroup.alpha = 0;

            if (_fadeType == Fade.FadeOut)
            {
                endValue = 0;
                _canvasGroup.alpha = 1;
            }

            _fadeTW = _canvasGroup.DOFade(endValue, _animationTime)
                .SetEase(_animationEase)
                .SetDelay(_startDelay);
        }

        public override void Stop() =>
            _fadeTW?.Kill();

        private enum Fade
        {
            FadeIn,
            FadeOut
        }
    }
}