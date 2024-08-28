using System.Collections;
using Scripts.Infrastructure.Actions;
using Scripts.UI.MainMenu.Levels;
using UnityEngine;

namespace Scripts.UI.MainMenu.Actions
{
    internal class UpdateLevelsButtonsState : IAction
    {
        private readonly LevelsPanel _levelsPanel;
        private readonly float _delay;
        private readonly bool _instant;

        public UpdateLevelsButtonsState(LevelsPanel levelsPanel, bool instant, float delay = 0)
        {
            _instant = instant;
            _delay = delay;
            _levelsPanel = levelsPanel;
        }

        public IEnumerator Execute()
        {
            yield return new WaitForSeconds(_delay);

            float scrollTime = _levelsPanel.UpdateInfo(_instant);

            yield return new WaitForSeconds(scrollTime);
        }
    }
}