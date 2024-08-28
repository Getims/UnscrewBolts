using System;
using UnityEngine;

namespace Scripts.GameLogic.Levels.Bolts
{
    [Serializable]
    internal class BoltAnimator
    {
        [SerializeField]
        private Transform _visualContainer;

        [SerializeField]
        private Transform _topContainer;

        [SerializeField]
        private Transform _modelContainer;

        [SerializeField]
        private ScrewAnimation _screwAnimation;

        public void Destroy()
        {
            _screwAnimation.Destroy();
        }

        public void Unscrew() => _screwAnimation.Unscrew(_visualContainer, _topContainer, _modelContainer);
        public void Screw() => _screwAnimation.Screw(_visualContainer, _topContainer, _modelContainer);
    }
}