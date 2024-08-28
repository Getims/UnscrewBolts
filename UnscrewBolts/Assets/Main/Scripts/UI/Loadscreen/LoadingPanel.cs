using System.Collections;
using System.Collections.Generic;
using Scripts.UI.Base;
using UnityEngine;

namespace Scripts.UI.Loadscreen
{
    public class LoadingPanel : UIPanel
    {
        [SerializeField]
        private List<UIPanel> _dots = new List<UIPanel>();

        private int _lastDot = 0;
        private int _currentDot = 0;
        private Coroutine _animationCO;

        public override void Show()
        {
            base.Show();

            if (_dots.Count == 0)
                return;

            if (_animationCO != null)
                StopCoroutine(_animationCO);

            _animationCO = StartCoroutine(Animation());
        }

        public override void Hide()
        {
            base.Hide();
            if (_animationCO != null)
                StopCoroutine(_animationCO);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_animationCO != null)
                StopCoroutine(_animationCO);
        }

        private IEnumerator Animation()
        {
            float time = _dots[0].FadeTime + 0.01f;
            _dots[0].Show();
            _lastDot = 0;
            _currentDot = 1;

            yield return new WaitForSeconds(time);

            while (true)
            {
                _dots[_lastDot].Hide();
                _dots[_currentDot].Show();
                _lastDot = _currentDot;
                _currentDot++;
                if (_currentDot >= _dots.Count)
                    _currentDot = 0;
                yield return new WaitForSeconds(time);
            }
        }
    }
}