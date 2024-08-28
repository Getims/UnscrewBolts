using UnityEngine;
using Zenject;

namespace Scripts.UI.Factories
{
    public interface IUIElementFactory
    {
        TElement Create<TElement>(TElement prefab) where TElement : MonoBehaviour;
        TElement Create<TElement>(TElement prefab, Transform parent) where TElement : MonoBehaviour;
    }

    public class UIElementFactory : IUIElementFactory
    {
        private readonly DiContainer _diContainer;

        public UIElementFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public TElement Create<TElement>(TElement prefab) where TElement : MonoBehaviour =>
            CreateElement(prefab, null);

        public TElement Create<TElement>(TElement prefab, Transform parent) where TElement : MonoBehaviour =>
            CreateElement(prefab, parent);

        private TElement CreateElement<TElement>(TElement prefab, Transform parent) where TElement : MonoBehaviour
        {
            if (parent == null)
                return _diContainer.InstantiatePrefabForComponent<TElement>(prefab);

            return _diContainer.InstantiatePrefabForComponent<TElement>(prefab, parent);
        }
    }
}