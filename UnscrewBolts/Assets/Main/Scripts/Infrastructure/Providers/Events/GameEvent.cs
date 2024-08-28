using System;
using System.Collections.Generic;

namespace Scripts.Infrastructure.Providers.Events
{
    public class GameEvent : IEvent
    {
        private readonly HashSet<Action> _listeners = new HashSet<Action>();

        public void AddListener(Action listener) =>
            _listeners.Add(listener);

        public void RemoveListener(Action listener) =>
            _listeners.Remove(listener);

        public void Invoke()
        {
            HashSet<Action> listenersCopy = new HashSet<Action>(_listeners);

            foreach (var listener in listenersCopy)
                listener?.Invoke();
        }

        public int ListenersCount() => _listeners.Count;
    }

    public class GameEvent<T> : IEvent
    {
        private readonly HashSet<Action<T>> _listeners = new HashSet<Action<T>>();

        public int ListenersCount() => _listeners.Count;

        public void AddListener(Action<T> listener) =>
            _listeners.Add(listener);

        public void RemoveListener(Action<T> listener) =>
            _listeners.Remove(listener);

        public void Invoke(T param)
        {
            HashSet<Action<T>> listenersCopy = new HashSet<Action<T>>(_listeners);

            foreach (var listener in listenersCopy)
                listener?.Invoke(param);
        }
    }
}