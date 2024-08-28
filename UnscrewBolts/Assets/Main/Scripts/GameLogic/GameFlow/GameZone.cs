using System.Collections.Generic;
using Scripts.GameLogic.Levels.GameElements;
using Scripts.Infrastructure.Providers.Events;
using UnityEngine;
using Zenject;

namespace Scripts.GameLogic.GameFlow
{
    public class GameZone : MonoBehaviour
    {
        [SerializeField]
        private List<SpriteRenderer> _helpers = new List<SpriteRenderer>();

        private LocalEventProvider _localEventProvider;

        [Inject]
        public void Construct(LocalEventProvider localEventProvider) =>
            _localEventProvider = localEventProvider;

        private void Awake()
        {
            foreach (SpriteRenderer helper in _helpers)
                helper.enabled = false;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out GameElement gameElement))
            {
                if (gameElement.IsDestroyed)
                    return;

                Vector3 position = ConvertWorldPointToScreen(other.transform);
                gameElement.DestroySelf();
                _localEventProvider.Invoke<GameElementDestroyEvent, Vector3>(position);
            }
        }

        private Vector3 ConvertWorldPointToScreen(Transform transformPoint) =>
            Camera.main.WorldToScreenPoint(transformPoint.position);
    }
}