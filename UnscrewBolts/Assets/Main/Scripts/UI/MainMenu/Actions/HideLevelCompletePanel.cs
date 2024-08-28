using System;
using System.Collections;
using Scripts.Infrastructure.Actions;
using UnityEngine;

namespace Scripts.UI.MainMenu.Actions
{
    public class HideLevelCompletePanel : IAction
    {
        private readonly Func<LevelCompletePanel> _getLevelCompletePanel;
        private readonly float _delay;

        public HideLevelCompletePanel(Func<LevelCompletePanel> getLevelCompletePanel, float delay)
        {
            _getLevelCompletePanel = getLevelCompletePanel;
            _delay = delay;
        }

        public IEnumerator Execute()
        {
            yield return new WaitForSeconds(_delay);

            LevelCompletePanel panel = _getLevelCompletePanel();
            panel.Hide();

            yield return new WaitForSeconds(panel.FadeTime);
        }
    }
}