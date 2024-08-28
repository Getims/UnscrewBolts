using System;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Infrastructure.Providers.Events
{
    public abstract class GameEventProvider
    {
        private readonly HashSet<IEvent> _events = new HashSet<IEvent>();

        public void AddListener<TEvent>(Action listener) where TEvent : GameEvent, new()
        {
            TEvent gameEvent = (TEvent) _events.FirstOrDefault(ev => ev is TEvent);

            if (gameEvent == null)
            {
                gameEvent = new TEvent();
                _events.Add(gameEvent);
            }

            gameEvent.AddListener(listener);
        }

        public void RemoveListener<TEvent>(Action listener) where TEvent : GameEvent
        {
            TEvent gameEvent = (TEvent) _events.FirstOrDefault(ev => ev is TEvent);
            gameEvent?.RemoveListener(listener);
            TryRemoveEvent(gameEvent);
        }

        public void Invoke<TEvent>() where TEvent : GameEvent
        {
            TEvent gameEvent = (TEvent) _events.FirstOrDefault(ev => ev is TEvent);
            gameEvent?.Invoke();
        }

        public void AddListener<TEvent, T>(Action<T> listener) where TEvent : GameEvent<T>, new()
        {
            TEvent gameEvent = (TEvent) _events.FirstOrDefault(ev => ev is TEvent);

            if (gameEvent == null)
            {
                gameEvent = new TEvent();
                _events.Add(gameEvent);
            }

            gameEvent.AddListener(listener);
        }

        public void RemoveListener<TEvent, T>(Action<T> listener) where TEvent : GameEvent<T>
        {
            TEvent gameEvent = (TEvent) _events.FirstOrDefault(ev => ev is TEvent);
            gameEvent?.RemoveListener(listener);
            TryRemoveEvent(gameEvent);
        }

        public void Invoke<TEvent, T>(T param) where TEvent : GameEvent<T>
        {
            TEvent gameEvent = (TEvent) _events.FirstOrDefault(ev => ev is TEvent);
            gameEvent?.Invoke(param);
        }

        private void TryRemoveEvent(IEvent gameEvent)
        {
            if (gameEvent == null)
                return;

            if (gameEvent.ListenersCount() == 0)
                _events.Remove(gameEvent);
        }
    }
}