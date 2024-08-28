namespace Scripts.Infrastructure.Providers.Events
{
    public class GameLoadCompleteEvent : GameEvent
    {
    }

    public class SoundSwitchEvent : GameEvent<bool>
    {
    }

    public class MusicSwitchEvent : GameEvent<bool>
    {
    }

    public class MainMenuButtonClickEvent : GameEvent
    {
    }

    public class PlayClickedEvent : GameEvent
    {
    }

    public class MoneyChangedEvent : GameEvent<int>
    {
    }

    public class LevelSwitchEvent : GameEvent<int>
    {
    }

    public class LevelStepSwitchEvent : GameEvent<int>
    {
    }

    public class ReloadLevelEvent : GameEvent
    {
    }
}