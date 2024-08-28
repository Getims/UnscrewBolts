using UnityEngine;

namespace Scripts.GameLogic.Levels.Bolts
{
    public readonly struct BoltMoveData
    {
        public Rigidbody2D BoltRigidbody { get; }
        public Collider2D BoltCollider { get; }
        public Vector2 BoltPosition { get; }
        public bool SetScrewed { get; }

        public BoltMoveData(Rigidbody2D boltRigidbody, Collider2D boltCollider, Vector2 boltPosition, bool setScrewed)
        {
            BoltRigidbody = boltRigidbody;
            BoltPosition = boltPosition;
            SetScrewed = setScrewed;
            BoltCollider = boltCollider;
        }
    }
}