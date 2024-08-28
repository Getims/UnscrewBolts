using Scripts.Core.Extensions;
using Scripts.GameLogic.GameFlow;
using TMPro;
using UnityEngine;
using Zenject;

namespace Scripts.UI.GameMenu
{
    internal class LevelTimer : MonoBehaviour
    {
        [SerializeField]
        private Color _baseTimeColor = Color.white;

        [SerializeField]
        private Color _noTimeColor = Color.red;

        [SerializeField]
        private TextMeshProUGUI _valueTMP;

        private IGameFlowProvider _gameFlowProvider;

        [Inject]
        public void Construct(IGameFlowProvider gameFlowProvider)
        {
            _gameFlowProvider = gameFlowProvider;
        }

        private void FixedUpdate()
        {
            if (_gameFlowProvider == null)
                return;

            UpdateTime();
        }

        private void UpdateTime()
        {
            int currentTime = (int) _gameFlowProvider.RemainLevelTime;
            UpdateTime(currentTime);
            _valueTMP.color = currentTime < 10 ? _noTimeColor : _baseTimeColor;
        }

        private void UpdateTime(int seconds)
        {
            NumbersExtensions.ConvertToMinutes(seconds, out string value);
            _valueTMP.text = value;
        }
    }
}