using System.Collections;
using Scripts.Data.Services;
using Scripts.Infrastructure.Actions;
using Scripts.UI.Common.FlyIcons;
using UnityEngine;

namespace Scripts.UI.GameMenu.Actions
{
    internal class ShowRewardAction : IAction
    {
        private readonly FlyIconAnimationHandler _flyIconHandler;
        private readonly Vector3 _from;
        private readonly Vector3 _to;
        private readonly IPlayerDataService _playerDataService;
        private readonly int _reward;

        public ShowRewardAction(Vector3 from, Vector3 to, FlyIconAnimationHandler flyIconHandler,
            IPlayerDataService playerDataService, int reward)
        {
            _reward = reward;
            _playerDataService = playerDataService;
            _flyIconHandler = flyIconHandler;
            _to = to;
            _from = from;
        }

        public IEnumerator Execute()
        {
            _flyIconHandler.StartAnimation(_from, _to);
            float actionTime = _flyIconHandler.GetAnimationTime();

            yield return new WaitForSeconds(actionTime);

            _playerDataService.AddMoney(_reward);
            yield return new WaitForEndOfFrame();
        }
    }
}