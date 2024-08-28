using System.Collections;
using Scripts.Data.Services;
using Scripts.Infrastructure.Actions;
using UnityEngine;

namespace Scripts.UI.MainMenu.Actions
{
    internal class SwitchLevel : IAction
    {
        private readonly IProgressDataService _progressDataService;
        private readonly float _actionTime;

        public SwitchLevel(IProgressDataService progressDataService, float actionTime = 0)
        {
            _progressDataService = progressDataService;
            _actionTime = actionTime;
        }

        public IEnumerator Execute()
        {
            _progressDataService.SwitchToNextLevel();
            _progressDataService.SetRewardState(false);
            yield return new WaitForSeconds(_actionTime);
        }
    }
}