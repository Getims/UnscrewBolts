using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.GameLogic.Levels.GameElements
{
    [Serializable]
    internal class LayerSetter
    {
        [SerializeField]
        private int _currentLayer = 0;

        public void Update(SpriteRenderer spriteRenderer, List<GameElementHole> elementHoles)
        {
            if (_currentLayer < 1)
                _currentLayer = 1;

            SetLayer(spriteRenderer, elementHoles);
        }

        public void Update(SpriteRenderer spriteRenderer, List<GameElementHole> elementHoles, int newLayer)
        {
            _currentLayer = newLayer;
            Update(spriteRenderer, elementHoles);
        }

        private void SetLayer(SpriteRenderer spriteRenderer, List<GameElementHole> elementHoles)
        {
            spriteRenderer.sortingOrder = _currentLayer;
            foreach (GameElementHole elementHole in elementHoles)
                elementHole.SetLayer(_currentLayer);
        }
    }
}