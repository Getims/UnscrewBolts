using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.UI.Common.FlyIcons
{
    [Serializable]
    public class IconsPool
    {
        [SerializeField]
        private List<FlyIcon> _flyIcons = new List<FlyIcon>();

        private int _currentIcon = 0;
        private int _iconsCount = 0;

        public void Initialize()
        {
            _currentIcon = 0;
            _iconsCount = _flyIcons.Count;

            foreach (FlyIcon icon in _flyIcons)
                icon.gameObject.SetActive(false);
        }

        public FlyIcon Spawn()
        {
            FlyIcon icon = _flyIcons[_currentIcon];
            icon.gameObject.SetActive(true);
            _currentIcon++;
            if (_currentIcon >= _iconsCount)
                _currentIcon = 0;

            return icon;
        }

        public void Despawn(FlyIcon icon)
        {
            icon.gameObject.SetActive(false);
        }
    }
}