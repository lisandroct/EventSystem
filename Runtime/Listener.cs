using lisandroct.EventSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace lisandroct.EventSystem
{
    public interface IListener { void OnEventRaised(); }
    
    public class Listener : MonoBehaviour, IListener
    {
        [FormerlySerializedAs("_gameEvent")] [FormerlySerializedAs("m_Event")] [SerializeField]
        private Event @event;
        private Event Event => @event;

        [FormerlySerializedAs("m_Response")] [SerializeField]
        private UnityEvent _response;
        private UnityEvent response => _response;

        private void OnEnable() {
            Event.RegisterListener(this);
        }

        private void OnDisable() {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised() {
            response.Invoke();
        }
    }
}