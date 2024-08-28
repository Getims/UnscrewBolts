using System;
using DG.Tweening;
using Scripts.UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Loadscreen
{
    public class LoadingProgressBarPanel : UIPanel
    {
        [SerializeField]
        private Image _progressFill;

        [SerializeField, Min(0)]
        private float _fillDelay = 0.1f;

        [SerializeField, Min(0)]
        private float _fillTime = 1f;

        [SerializeField]
        private Ease _fillEase = Ease.InOutBounce;

        private Tweener _fillTW;

        public void Fill(Action onFillComplete)
        {
            DontDestroyOnLoad(this);
            _fillTW?.Kill();
            _progressFill.fillAmount = 0;

            _fillTW = _progressFill.DOFillAmount(.5f, _fillTime * .5f)
                .SetDelay(_fillDelay)
                .SetEase(_fillEase)
                .OnComplete(() =>
                {
                    onFillComplete?.Invoke();
                    Step2Fill();
                });
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _fillTW?.Kill();
        }

        private void Step2Fill()
        {
            _fillTW = _progressFill.DOFillAmount(1f, _fillTime * .5f)
                .SetEase(_fillEase)
                .SetDelay(_fillDelay)
                .OnComplete(() =>
                {
                    Hide(false);
                    DestroySelfDelayed();
                });
        }
    }
}