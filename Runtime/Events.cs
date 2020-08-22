using System;
using UnityEngine;

namespace lisandroct.EventSystem
{
    public abstract class Event<T> : ScriptableObject
    {
        private event Action<T> OnEvent;
        public int HandlersCount => OnEvent?.GetInvocationList().Length ?? 0;

        public void Invoke(T arg) => OnEvent?.Invoke(arg);

        public void Register(IListener<T> listener) => Register(listener.OnEventRaised);
        public void Register(Action<T> handler)
        {
            OnEvent -= handler;
            OnEvent += handler;
        }

        public void Unregister(IListener<T> listener) => Unregister(listener.OnEventRaised);
        public void Unregister(Action<T> handler) => OnEvent -= handler;
    }

    public abstract class Event<T, U> : ScriptableObject
    {
        private event Action<T, U> OnEvent;
        public int HandlersCount => OnEvent?.GetInvocationList().Length ?? 0;

        public void Invoke(T arg0, U arg1) => OnEvent?.Invoke(arg0, arg1);

        public void Register(IListener<T, U> listener) => Register(listener.OnEventRaised);
        public void Register(Action<T, U> listener)
        {
            OnEvent -= listener;
            OnEvent += listener;
        }

        public void Unregister(IListener<T, U> listener) => Unregister(listener.OnEventRaised);
        public void Unregister(Action<T, U> listener) => OnEvent -= listener;
}

    public abstract class Event<T, U, V> : ScriptableObject
    {
        private event Action<T, U, V> OnEvent;
        public int HandlersCount => OnEvent?.GetInvocationList().Length ?? 0;

        public void Invoke(T arg0, U arg1, V arg2) => OnEvent?.Invoke(arg0, arg1, arg2);

        public void Register(IListener<T, U, V> listener) => Register(listener.OnEventRaised);
        public void Register(Action<T, U, V> listener)
        {
            OnEvent -= listener;
            OnEvent += listener;
        }

        public void Unregister(IListener<T, U, V> listener) => Unregister(listener.OnEventRaised);
        public void Unregister(Action<T, U, V> listener) => OnEvent -= listener;
    }

    public abstract class Event<T, U, V, W> : ScriptableObject
    {
        private event Action<T, U, V, W> OnEvent;
        public int HandlersCount => OnEvent?.GetInvocationList().Length ?? 0;

        public void Invoke(T arg0, U arg1, V arg2, W arg3) => OnEvent?.Invoke(arg0, arg1, arg2, arg3);
        
        public void Register(IListener<T, U, V, W> listener) => Register(listener.OnEventRaised);
        public void Register(Action<T, U, V, W> listener)
        {
            OnEvent -= listener;
            OnEvent += listener;
        }

        public void Unregister(IListener<T, U, V, W> listener) => Unregister(listener.OnEventRaised);
        public void Unregister(Action<T, U, V, W> listener) => OnEvent -= listener;
    }
}