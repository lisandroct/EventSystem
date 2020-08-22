using UnityEngine;
using UnityEngine.Events;

namespace lisandroct.EventSystem
{
    public interface IListener { void OnEventRaised(); }
    
    #if UNITY_2020_1_OR_NEWER
    public class Listener : MonoBehaviour, IListener
    {
        [SerializeField]
        private Event _event;
        private Event Event => _event;

        [SerializeField]
        private UnityEvent _response;
        private UnityEvent Response => _response;

        private void OnEnable() {
            Event?.Register(this);
        }

        private void OnDisable() {
            Event?.Unregister(this);
        }

        public void OnEventRaised() => Response?.Invoke();
    }
    #else
    public class Listener : MonoBehaviour, IListener
    {
        [SerializeField]
        private Event _event;
        private Event Event => _event;

        [SerializeField]
        private UnityEvent _response;
        private UnityEvent Response => _response;

        private void OnEnable() {
            Event?.RegisterListener(this);
        }

        private void OnDisable() {
            Event?.UnregisterListener(this);
        }

        public void OnEventRaised() => Response?.Invoke();
    }
    #endif
}