using System;
using System.Collections.Generic;
using UnityEngine;

namespace lisandroct.EventSystem
{
    public abstract class Event<T> : ScriptableObject
    {
        private List<IListener<T>> listeners { get; } = new List<IListener<T>>();
        public int ListenersCount => listeners.Count;

        public void Raise(T element) {
            for(int i = listeners.Count - 1; i >= 0; i--) {
                listeners[i].OnEventRaised(element);
            }
        }
        
        public void RegisterListener(IListener<T> listener) {
            if(listeners.Contains(listener)) {
                return;
            }

            listeners.Add(listener);
        }

        public void UnregisterListener(IListener<T> listener) {
            if(!listeners.Contains(listener)) {
                return;
            }

            listeners.Remove(listener);
        }
    }

    public abstract class Event<T, U> : ScriptableObject
    {
        private List<IListener<T, U>> listeners { get; } = new List<IListener<T, U>>();
        public int ListenersCount => listeners.Count;

        public void Raise(T element0, U element1) {
            for(int i = listeners.Count - 1; i >= 0; i--) {
                listeners[i].OnEventRaised(element0, element1);
            }
        }

        public void RegisterListener(IListener<T, U> listener) {
            if(listeners.Contains(listener)) {
                return;
            }

            listeners.Add(listener);
        }

        public void UnregisterListener(IListener<T, U> listener) {
            if(!listeners.Contains(listener)) {
                return;
            }

            listeners.Remove(listener);
        }
    }

    public abstract class Event<T, U, V> : ScriptableObject
    {
        private List<IListener<T, U, V>> listeners { get; } = new List<IListener<T, U, V>>();
        public int ListenersCount => listeners.Count;

        public void Raise(T element0, U element1, V element2) {
            for(int i = listeners.Count - 1; i >= 0; i--) {
                listeners[i].OnEventRaised(element0, element1, element2);
            }
        }

        public void RegisterListener(IListener<T, U, V> listener) {
            if(listeners.Contains(listener)) {
                return;
            }

            listeners.Add(listener);
        }

        public void UnregisterListener(IListener<T, U, V> listener) {
            if(!listeners.Contains(listener)) {
                return;
            }

            listeners.Remove(listener);
        }
    }

    public abstract class Event<T, U, V, W> : ScriptableObject
    {
        private List<IListener<T, U, V, W>> listeners { get; } = new List<IListener<T, U, V, W>>();
        public int ListenersCount => listeners.Count;

        public void Raise(T element0, U element1, V element2, W element3) {
            for(int i = listeners.Count - 1; i >= 0; i--) {
                listeners[i].OnEventRaised(element0, element1, element2, element3);
            }
        }

        public void RegisterListener(IListener<T, U, V, W> listener) {
            if(listeners.Contains(listener)) {
                return;
            }

            listeners.Add(listener);
        }

        public void UnregisterListener(IListener<T, U, V, W> listener) {
            if(!listeners.Contains(listener)) {
                return;
            }

            listeners.Remove(listener);
        }
    }
}