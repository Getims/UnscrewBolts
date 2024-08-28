using System;
using UnityEngine;

namespace Scripts.UI.Tutorial
{
    [Serializable]
    public class TutorialStep
    {
        [SerializeField]
        private String _title;

        [SerializeField]
        private StepType _stepType;

        [SerializeField]
        private Vector2 _boltPosition;

        public string Title => _title;
        public StepType StepType => _stepType;
        public Vector2 BoltPosition => _boltPosition;
    }
}