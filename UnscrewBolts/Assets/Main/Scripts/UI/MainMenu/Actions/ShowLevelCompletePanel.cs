using System;
using System.Collections;
using Scripts.Infrastructure.Actions;
using UnityEngine;

namespace Scripts.UI.MainMenu.Actions
{
    internal class ShowLevelCompletePanel : IAction
    {
        private readonly Func<LevelCompletePanel> _getLevelCompletePanel;

        public ShowLevelCompletePanel(Func<LevelCompletePanel> getLevelCompletePanel)
        {
            _getLevelCompletePanel = getLevelCompletePanel;
        }

        public IEnumerator Execute()
        {
            LevelCompletePanel panel = _getLevelCompletePanel();
            panel.Show();
            float showTime = panel.GetShowTime();
            yield return new WaitForSeconds(showTime);
        }
    }
}