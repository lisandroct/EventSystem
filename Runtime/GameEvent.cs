using System.Collections.Generic;
using UnityEngine;

namespace lisandroct.EventSystem
{
    public class GameEvent : ScriptableObject
    {
        private List<IGameEventListener> listeners { get; } = new List<IGameEventListener>();
        public int ListenersCount { get { return listeners.Count; } }

        public void Raise() {
            for(int i = listeners.Count - 1; i >= 0; i--) {
                listeners[i].OnEventRaised();
            }
        }

        public void RegisterListener(IGameEventListener listener) {
            if(listeners.Contains(listener)) {
                return;
            }

            listeners.Add(listener);
        }

        public void UnregisterListener(IGameEventListener listener) {
            if(!listeners.Contains(listener)) {
                return;
            }

            listeners.Remove(listener);
        }
    }

    public abstract class GameEvent<T> : ScriptableObject
    {
        private List<IGameEventListener<T>> listeners { get; } = new List<IGameEventListener<T>>();
        public int ListenersCount { get { return listeners.Count; } }

        public void Raise(T element) {
            for(int i = listeners.Count - 1; i >= 0; i--) {
                listeners[i].OnEventRaised(element);
            }
        }

        public void RegisterListener(IGameEventListener<T> listener) {
            if(listeners.Contains(listener)) {
                return;
            }

            listeners.Add(listener);
        }

        public void UnregisterListener(IGameEventListener<T> listener) {
            if(!listeners.Contains(listener)) {
                return;
            }

            listeners.Remove(listener);
        }
    }

    public abstract class GameEvent<T, U> : ScriptableObject
    {
        private List<IGameEventListener<T, U>> listeners { get; } = new List<IGameEventListener<T, U>>();
        public int ListenersCount { get { return listeners.Count; } }

        public void Raise(T element0, U element1) {
            for(int i = listeners.Count - 1; i >= 0; i--) {
                listeners[i].OnEventRaised(element0, element1);
            }
        }

        public void RegisterListener(IGameEventListener<T, U> listener) {
            if(listeners.Contains(listener)) {
                return;
            }

            listeners.Add(listener);
        }

        public void UnregisterListener(IGameEventListener<T, U> listener) {
            if(!listeners.Contains(listener)) {
                return;
            }

            listeners.Remove(listener);
        }
    }

    public abstract class GameEvent<T, U, V> : ScriptableObject
    {
        private List<IGameEventListener<T, U, V>> listeners { get; } = new List<IGameEventListener<T, U, V>>();
        public int ListenersCount { get { return listeners.Count; } }

        public void Raise(T element0, U element1, V element2) {
            for(int i = listeners.Count - 1; i >= 0; i--) {
                listeners[i].OnEventRaised(element0, element1, element2);
            }
        }

        public void RegisterListener(IGameEventListener<T, U, V> listener) {
            if(listeners.Contains(listener)) {
                return;
            }

            listeners.Add(listener);
        }

        public void UnregisterListener(IGameEventListener<T, U, V> listener) {
            if(!listeners.Contains(listener)) {
                return;
            }

            listeners.Remove(listener);
        }
    }

    public abstract class GameEvent<T, U, V, W> : ScriptableObject
    {
        private List<IGameEventListener<T, U, V, W>> listeners { get; } = new List<IGameEventListener<T, U, V, W>>();
        public int ListenersCount { get { return listeners.Count; } }

        public void Raise(T element0, U element1, V element2, W element3) {
            for(int i = listeners.Count - 1; i >= 0; i--) {
                listeners[i].OnEventRaised(element0, element1, element2, element3);
            }
        }

        public void RegisterListener(IGameEventListener<T, U, V, W> listener) {
            if(listeners.Contains(listener)) {
                return;
            }

            listeners.Add(listener);
        }

        public void UnregisterListener(IGameEventListener<T, U, V, W> listener) {
            if(!listeners.Contains(listener)) {
                return;
            }

            listeners.Remove(listener);
        }
    }
}