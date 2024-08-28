using UnityEngine;

namespace Scripts.UI
{
    public class UIContainerProvider : MonoBehaviour
    {
        [SerializeField]
        private Transform _menuContainer;

        [SerializeField]
        private Transform _windowsContainer;

        [SerializeField]
        private Transform _coinsContainer;

        public Transform MenuContainer => _menuContainer;
        public Transform WindowsContainer => _windowsContainer;
        public Transform CoinsContainer => _coinsContainer;
    }
}