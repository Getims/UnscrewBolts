using System.Collections;
using Scripts.Infrastructure.Actions;
using Scripts.UI.MainMenu.Prize;
using UnityEngine;

namespace Scripts.UI.MainMenu.Actions
{
    internal class ShowPrizePanel : IAction
    {
        private readonly int _prizeReward;
        private readonly float _delay;
        private readonly PrizePanel _prizePanel;

        private bool _prizeCollected;

        public ShowPrizePanel(PrizePanel prizePanel, int prizeReward, float delay)
        {
            _prizePanel = prizePanel;
            _delay = delay;
            _prizeReward = prizeReward;
        }

        public IEnumerator Execute()
        {
            yield return new WaitForSeconds(_delay);

            _prizePanel.Initialize(_prizeReward);
            _prizePanel.Show();
            _prizePanel.OnPrizeCollectClick += OnPrizeCollect;
            WaitForSeconds wait = new WaitForSeconds(0.1f);

            while (!_prizeCollected)
                yield return wait;

            _prizePanel.OnPrizeCollectClick -= OnPrizeCollect;
        }

        private void OnPrizeCollect() => _prizeCollected = true;
    }
}