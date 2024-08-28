using DG.Tweening;
using Scripts.Core.Constants;
using UnityEngine;

namespace Scripts.GameLogic.Levels.Core
{
    public abstract class VisualElement : MonoBehaviour
    {
        [SerializeField]
        protected SpriteRenderer _sprite;

        private Tweener _fadeTN;
        private bool _instant;

        public abstract void Initialize();

        public virtual void Show(bool instant = false)
        {
            _instant = instant;
            VisibilityState(true);
        }

        public virtual void Hide(bool instant = false)
        {
            _instant = instant;
            VisibilityState(false);
        }

        protected virtual void OnDestroy()
        {
            _fadeTN.Kill();
        }

        private void SetSpriteAlpha(float alpha)
        {
            Color newColor = _sprite.color;
            newColor.a = alpha;
            _sprite.color = newColor;
        }

        private void VisibilityState(bool show)
        {
            float duration = GameLogicConstants.ELEMENT_FADE_TIME;
            float value = show ? 1 : 0;
            _fadeTN.Kill();

            if (_instant)
                SetSpriteAlpha(value);
            else
                _fadeTN = _sprite.DOFade(value, duration);
        }
    }
}