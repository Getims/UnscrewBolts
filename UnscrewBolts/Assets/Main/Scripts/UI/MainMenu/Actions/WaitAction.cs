using System.Collections;
using Scripts.Infrastructure.Actions;
using UnityEngine;

namespace Scripts.UI.MainMenu.Actions
{
    internal class WaitAction : IAction
    {
        private readonly float _waitTime;

        public WaitAction(float waitTime = 0) =>
            _waitTime = waitTime;

        public IEnumerator Execute()
        {
            yield return new WaitForSeconds(_waitTime);
        }
    }
}