using System.Collections;
using Scripts.Infrastructure.Actions;
using Scripts.UI.MainMenu.Levels;
using UnityEngine;

namespace Scripts.UI.MainMenu.Actions
{
    internal class HideRewardPanel : IAction
    {
        private readonly LevelsPanel _levelsPanel;
        private readonly float _delay;

        public HideRewardPanel(LevelsPanel levelsPanel, float delay = 0)
        {
            _delay = delay;
            _levelsPanel = levelsPanel;
        }

        public IEnumerator Execute()
        {
            yield return new WaitForSeconds(_delay);

            _levelsPanel.HideRewardPanel();
            yield return new WaitForEndOfFrame();
        }
    }
}