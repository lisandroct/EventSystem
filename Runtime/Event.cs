using System;
using UnityEngine;

namespace lisandroct.EventSystem
{
    [CreateAssetMenu(fileName = "OnGameEvent", menuName = "Events/Game Event")]
    public class Event : ScriptableObject
    {
        private event Action OnEvent;
        public int HandlersCount => OnEvent?.GetInvocationList().Length ?? 0;

        public void Invoke() => OnEvent?.Invoke();

        public void Register(IListener listener) => Register(listener.OnEventRaised);
        public void Register(Action handler)
        {
            OnEvent -= handler;
            OnEvent += handler;
        }

        public void Unregister(IListener listener) => Unregister(listener.OnEventRaised);
        public void Unregister(Action handler) => OnEvent -= handler;
    }
}