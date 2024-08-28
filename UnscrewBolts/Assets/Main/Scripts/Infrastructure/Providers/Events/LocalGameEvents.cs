using Scripts.GameLogic.Levels.Anchors;
using Scripts.GameLogic.Levels.Bolts;
using UnityEngine;

namespace Scripts.Infrastructure.Providers.Events
{
    public class ThemeSwitchEvent : GameEvent
    {
    }

    public class GameOverEvent : GameEvent<bool>
    {
    }

    public class BoltMoveEvent : GameEvent<BoltMoveData>
    {
    }

    public class AnchorClickEvent : GameEvent<AnchorClickData>
    {
    }

    public class GameElementDestroyEvent : GameEvent<Vector3>
    {
    }

    public class TryToUnlockAnchorEvent : GameEvent<IAnchorStateSetter>
    {
    }

    public class UnscrewBoosterUseEvent : GameEvent<bool>
    {
    }

    public class RemoveBoltEvent : GameEvent
    {
    }

    public class BombBoosterUseEvent : GameEvent
    {
    }

    public class BombExplodeEvent : GameEvent
    {
    }
}