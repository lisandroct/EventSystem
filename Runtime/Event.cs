using System.Collections.Generic;
using UnityEngine;

namespace lisandroct.EventSystem
{
    [CreateAssetMenu(fileName = "OnGameEvent", menuName = "Events/Game Event")]
    public class Event : ScriptableObject
    {
        private List<IListener> listeners { get; } = new List<IListener>();
        public int ListenersCount => listeners.Count;

        public void Raise() {
            for(int i = listeners.Count - 1; i >= 0; i--) {
                listeners[i].OnEventRaised();
            }
        }

        public void RegisterListener(IListener listener) {
            if(listeners.Contains(listener)) {
                return;
            }

            listeners.Add(listener);
        }

        public void UnregisterListener(IListener listener) {
            if(!listeners.Contains(listener)) {
                return;
            }

            listeners.Remove(listener);
        }
    }
}