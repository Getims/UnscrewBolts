using System;
using System.Collections;
using Scripts.Infrastructure.Actions;
using Scripts.UI.Common.FlyIcons;
using UnityEngine;

namespace Scripts.UI.MainMenu.Actions
{
    internal class ShowRewardAction : IAction
    {
        private readonly FlyIconAnimationHandler _flyIconHandler;
        private readonly float _delay;
        private readonly Func<Vector3> _from;
        private readonly Func<Vector3> _to;

        public ShowRewardAction(Func<Vector3> from, Func<Vector3> to,
            FlyIconAnimationHandler flyIconHandler, float delay = 0)
        {
            _to = to;
            _from = from;
            _delay = delay;
            _flyIconHandler = flyIconHandler;
        }

        public IEnumerator Execute()
        {
            yield return new WaitForSeconds(_delay);

            _flyIconHandler.StartAnimation(_from(), _to());
            float actionTime = _flyIconHandler.GetAnimationTime();

            yield return new WaitForSeconds(actionTime);
        }
    }
}