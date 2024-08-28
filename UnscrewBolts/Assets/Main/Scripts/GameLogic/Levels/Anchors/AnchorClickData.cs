using UnityEngine;

namespace Scripts.GameLogic.Levels.Anchors
{
    public readonly struct AnchorClickData
    {
        public Vector2 AnchorPosition { get; }
        public bool SetScrewed { get; }

        public AnchorClickData(Vector2 anchorPosition, bool setScrewed)
        {
            AnchorPosition = anchorPosition;
            SetScrewed = setScrewed;
        }
    }
}