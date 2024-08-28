using System.Collections.Generic;
using Scripts.GameLogic.Levels.Bolts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.GameLogic.Levels.Anchors
{
    public class AnchorsController : MonoBehaviour
    {
        [SerializeField]
        private List<AnchorPoint> _anchorPoints = new List<AnchorPoint>();

        public void Initialize()
        {
            foreach (AnchorPoint anchorPoint in _anchorPoints)
                anchorPoint.Initialize();
        }

        public void ShowAnchors()
        {
            foreach (AnchorPoint anchorPoint in _anchorPoints)
                anchorPoint.Show();
        }

        public void HideAnchors(bool instant)
        {
            foreach (AnchorPoint anchorPoint in _anchorPoints)
                anchorPoint.Hide(instant);
        }

        public List<Bolt> GetBolts()
        {
            List<Bolt> _activeBolts = new List<Bolt>(_anchorPoints.Count);
            foreach (AnchorPoint anchorPoint in _anchorPoints)
            {
                Bolt bolt = anchorPoint.Bolt;
                if (bolt != null)
                    _activeBolts.Add(bolt);
            }

            return _activeBolts;
        }

        [Button]
        public void CollectAnchors()
        {
#if UNITY_EDITOR
            AnchorPoint[] anchorPoints = GetComponentsInChildren<AnchorPoint>();
            _anchorPoints.Clear();
            _anchorPoints.AddRange(anchorPoints);
#endif
        }
    }
}