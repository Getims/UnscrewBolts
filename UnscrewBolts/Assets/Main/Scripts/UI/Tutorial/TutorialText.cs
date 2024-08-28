using Scripts.Core.Utilities;
using Scripts.UI.Base;
using Scripts.UI.Common.UIAnimator.Main;
using TMPro;
using UnityEngine;

namespace Scripts.UI.Tutorial
{
    public class TutorialText : UIPanel
    {
        [SerializeField]
        private RectTransform _container;

        [SerializeField]
        private TextMeshProUGUI _title;

        [SerializeField]
        private UIAnimator _titleAnimator;

        public void ShowText(string title, Vector2 boltPosition)
        {
            Utils.ReworkPoint("Add tutorial translation");
            _title.text = title;
            MoveTo(boltPosition);
            _titleAnimator.Play();
        }

        public override void Hide()
        {
            base.Hide();
            _titleAnimator.Stop();
        }

        private void MoveTo(Vector2 boltPosition)
        {
            Vector2 position = ConvertWorldPointToScreen(boltPosition);
            _container.position = position;
        }

        private Vector2 ConvertWorldPointToScreen(Vector2 point) =>
            Camera.main.WorldToScreenPoint(point);
    }
}