using System.Collections;
using Scripts.Infrastructure.Actions;
using Scripts.UI.MainMenu.Levels;
using UnityEngine;

namespace Scripts.UI.MainMenu.Actions
{
    internal class ShowRewardPanel : IAction
    {
        private readonly LevelsPanel _levelsPanel;
        private readonly float _delay;
        private readonly int _reward;

        public ShowRewardPanel(LevelsPanel levelsPanel, int reward, float delay = 0)
        {
            _reward = reward;
            _delay = delay;
            _levelsPanel = levelsPanel;
        }

        public IEnumerator Execute()
        {
            yield return new WaitForSeconds(_delay);

            _levelsPanel.ShowRewardPanel(_reward);
            yield return new WaitForEndOfFrame();
        }
    }
}