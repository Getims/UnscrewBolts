using System.Collections.Generic;
using UnityEngine;

namespace Scripts.GameLogic.Levels.Bolts
{
    public class BoltsController : MonoBehaviour
    {
        [SerializeField]
        private Transform _boltsContainer;

        private List<Bolt> _bolts = new List<Bolt>();

        public void Initialize(List<Bolt> bolts)
        {
            _bolts.AddRange(bolts);
            foreach (Bolt bolt in _bolts)
            {
                MoveBoltToContainer(bolt.transform);
                bolt.Initialize();
            }
        }

        public void ShowBolts()
        {
            foreach (Bolt bolt in _bolts)
            {
                if (bolt != null)
                    bolt.Show();
            }
        }

        public void HideBolts(bool instant)
        {
            foreach (Bolt bolt in _bolts)
            {
                if (bolt != null)
                    bolt.Hide(instant);
            }
        }

        private void MoveBoltToContainer(Transform bolt)
        {
            bolt.SetParent(_boltsContainer);

            Vector3 boltPosition = bolt.localPosition;
            boltPosition.z = 0;
            bolt.localPosition = boltPosition;
        }
    }
}