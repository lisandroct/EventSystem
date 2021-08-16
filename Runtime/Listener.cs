using UnityEngine;
using UnityEngine.Events;

namespace lisandroct.EventSystem
{
    public interface IListener { void OnEventRaised(); }
    
    #if UNITY_2020_1_OR_NEWER
    public class Listener : MonoBehaviour, IListener
    {
        [SerializeField]
        private GameEvent _event;
        private GameEvent Event => _event;

        [SerializeField]
        private UnityEvent _response;
        private UnityEvent Response => _response;

        private void OnEnable() {
            if (Event != null)
            {
                Event.Register(this);
            }
        }

        private void OnDisable() {
            if (Event != null)
            {
                Event.Unregister(this);
            }
        }

        public void OnEventRaised() => Response?.Invoke();
    }
    #else
    public class Listener : MonoBehaviour, IListener
    {
        [SerializeField]
        private GameEvent _event;
        private GameEvent Event => _event;

        [SerializeField]
        private UnityEvent _response;
        private UnityEvent Response => _response;

        private void OnEnable() {
            if (Event != null)
            {
                Event.Register(this);
            }
        }

        private void OnDisable() {
            if (Event != null)
            {
                Event.Unregister(this);
            }
        }

        public void OnEventRaised() => Response?.Invoke();
    }
    #endif
}