using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace lisandroct.EventSystem
{
    public interface IListener<in T0> { void OnEventRaised(T0 element); }
    public interface IListener<in T0, in T1> { void OnEventRaised(T0 element0, T1 element1); }
    public interface IListener<in T0, in T1, in T2> { void OnEventRaised(T0 element0, T1 element1, T2 element2); }
    public interface IListener<in T0, in T1, in T2, in T3> { void OnEventRaised(T0 element0, T1 element1, T2 element2, T3 element3); }

    public abstract class Listener<T> : MonoBehaviour, IListener<T> 
    {
        [SerializeField]
        private Event<T> _event;
        private Event<T> Event => _event;

        [SerializeField]
        private UnityEvent<T> _response;
        private UnityEvent<T> response => _response;

        private void OnEnable() {
            Event?.RegisterListener(this);
        }

        private void OnDisable() {
            Event?.UnregisterListener(this);
        }

        public void OnEventRaised(T element) {
            response?.Invoke(element);
        }
    }

    public abstract class Listener<T0, T1> : MonoBehaviour, IListener<T0, T1>
    {
        [SerializeField]
        private Event<T0, T1> _event;
        private Event<T0, T1> Event => _event;

        [SerializeField]
        private UnityEvent<T0, T1> _response;
        private UnityEvent<T0, T1> response => _response;

        private void OnEnable() {
            Event?.RegisterListener(this);
        }

        private void OnDisable() {
            Event?.UnregisterListener(this);
        }

        public void OnEventRaised(T0 element0, T1 element1) {
            response?.Invoke(element0, element1);
        }
    }

    public abstract class Listener<T0, T1, T2> : MonoBehaviour, IListener<T0, T1, T2>
    {
        [SerializeField]
        private Event<T0, T1, T2> _event;
        private Event<T0, T1, T2> Event => _event;

        [SerializeField]
        private UnityEvent<T0, T1, T2> _response;
        private UnityEvent<T0, T1, T2> response => _response;

        private void OnEnable() {
            Event?.RegisterListener(this);
        }

        private void OnDisable() {
            Event?.UnregisterListener(this);
        }

        public void OnEventRaised(T0 element0, T1 element1, T2 element2) {
            response?.Invoke(element0, element1, element2);
        }
    }

    public abstract class Listener<T0, T1, T2, T3> : MonoBehaviour, IListener<T0, T1, T2, T3>
    {
        [SerializeField]
        private Event<T0, T1, T2, T3> _event;
        private Event<T0, T1, T2, T3> Event => _event;

        [SerializeField]
        private UnityEvent<T0, T1, T2, T3> _response;
        private UnityEvent<T0, T1, T2, T3> response => _response;

        private void OnEnable() {
            Event?.RegisterListener(this);
        }

        private void OnDisable() {
            Event?.UnregisterListener(this);
        }

        public void OnEventRaised(T0 element0, T1 element1, T2 element2, T3 element3) {
            response?.Invoke(element0, element1, element2, element3);
        }
    }
}