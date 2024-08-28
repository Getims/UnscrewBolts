using System;
using UnityEngine;

namespace Scripts.Data
{
    [Serializable]
    public class LevelData
    {
        [SerializeField]
        private int _id;

        [SerializeField]
        private bool _isUnlocked;

        [SerializeField]
        private bool _isComplete;

        public bool IsUnlocked => _isUnlocked;
        public bool IsLevelComplete() => _isComplete;

        private string ElementName =>
            $"Level_{_id + 1}         Is unlocked: {_isUnlocked} | Is complete: {_isComplete}";

        public LevelData(int id) => _id = id;

        internal void SetLockState(bool isUnlocked) =>
            _isUnlocked = isUnlocked;

        internal void SetCompleteState(bool isComplete) =>
            _isComplete = isComplete;
    }
}