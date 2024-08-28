using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.UI.Base
{
    public class UIPanel : MonoBehaviour
    {
        private const string SETTINGS = "Settings";

        [FoldoutGroup(SETTINGS)]
        [SerializeField, Min(0)]
        private float _fadeTime = 0.35f;

        [FoldoutGroup(SETTINGS)]
        [SerializeField, Required]
        protected CanvasGroup _targetCG;

        [FoldoutGroup(SETTINGS)]
        [SerializeField]
        private bool _ignoreScaleTime;

        [FoldoutGroup(SETTINGS)]
        [SerializeField]
        private bool _hideOnAwake;

        [FoldoutGroup(SETTINGS)]
        [SerializeField]
        private bool _useCanvas;

        [FoldoutGroup(SETTINGS)]
        [SerializeField]
        [ShowIf(nameof(_useCanvas))]
        private Canvas _canvas;

        private Tweener _fadeTN;
        private bool _instant;

        public event Action HideEvent;
        public float FadeTime => _fadeTime;

        private void OnValidate() =>
            TryGetComponent(out _targetCG);

        protected virtual void Awake()
        {
            if (_hideOnAwake)
                _targetCG.alpha = 0;
        }

        protected virtual void OnDestroy()
        {
            _fadeTN.Kill();
            CancelInvoke(nameof(DestroySelf));
        }

        public virtual void Show()
        {
            _instant = false;

            SetCanvasState(true);
            VisibilityState(true);
        }

        public virtual void Show(bool instant)
        {
            _instant = instant;

            SetCanvasState(true);
            VisibilityState(true);
        }

        public virtual void Show(float delay)
        {
            _instant = false;

            SetCanvasState(true);
            VisibilityState(true, delay);
        }

        public virtual void Hide()
        {
            _instant = false;

            VisibilityState(false);
        }

        public void Hide(float delay)
        {
            _instant = false;

            VisibilityState(false, delay);
        }

        public virtual void Hide(bool instant)
        {
            _instant = instant;

            VisibilityState(false);
        }

        protected void DestroySelf()
        {
            try
            {
                _fadeTN.Kill();
                if (gameObject != null)
                    Destroy(gameObject);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        protected void DestroySelfDelayed()
        {
            Invoke(nameof(DestroySelf), FadeTime);
        }

        private void VisibilityState(bool show, float delay = 0)
        {
            float duration = _fadeTime;
            float value = show ? 1 : 0;
            _fadeTN.Kill();

            if (_instant)
            {
                _targetCG.alpha = value;
                _targetCG.interactable = show;
                _targetCG.blocksRaycasts = show;
                return;
            }

            if (_targetCG == null)
                return;

            _fadeTN = _targetCG
                .DOFade(value, duration)
                .SetUpdate(_ignoreScaleTime)
                .SetDelay(delay)
                .OnComplete(() =>
                {
                    if (show)
                        return;

                    HideEvent?.Invoke();
                    SetCanvasState(false);
                });

            _targetCG.interactable = show;
            _targetCG.blocksRaycasts = show;
        }

        private void SetCanvasState(bool isEnabled)
        {
            if (!_useCanvas)
                return;

            _canvas.enabled = isEnabled;
        }
    }
}