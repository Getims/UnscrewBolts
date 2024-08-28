using System.Collections;
using Scripts.Data.Services;
using Scripts.Infrastructure.Actions;
using UnityEngine;

namespace Scripts.UI.MainMenu.Actions
{
    internal class AddMoneyAction : IAction
    {
        private readonly IPlayerDataService _playerDataService;
        private readonly int _reward;

        public AddMoneyAction(IPlayerDataService playerDataService, int reward)
        {
            _reward = reward;
            _playerDataService = playerDataService;
        }

        public IEnumerator Execute()
        {
            _playerDataService.AddMoney(_reward);
            yield return new WaitForEndOfFrame();
        }
    }
}