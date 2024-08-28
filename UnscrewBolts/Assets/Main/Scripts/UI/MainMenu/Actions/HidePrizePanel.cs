using System.Collections;
using Scripts.Infrastructure.Actions;
using Scripts.UI.MainMenu.Prize;
using UnityEngine;

namespace Scripts.UI.MainMenu.Actions
{
    internal class HidePrizePanel : IAction
    {
        private readonly PrizePanel _prizePanel;
        private readonly float _delay;

        public HidePrizePanel(PrizePanel prizePanel, float delay)
        {
            _delay = delay;
            _prizePanel = prizePanel;
        }

        public IEnumerator Execute()
        {
            yield return new WaitForSeconds(_delay);

            _prizePanel.Hide();
            yield return new WaitForSeconds(_prizePanel.FadeTime);
        }
    }
}