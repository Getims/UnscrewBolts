using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.GameMenu
{
    internal class Step : MonoBehaviour
    {
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private float _showTime = 0.25f;

        private Tweener _showTW;

        public void Show()
        {
            _showTW?.Kill();
            _showTW = _icon.DOFade(1, _showTime);
        }

        public void Hide()
        {
            _showTW?.Kill();
            _showTW = _icon.DOFade(0, 0);
        }

        private void OnDestroy() => _showTW?.Kill();
    }
}