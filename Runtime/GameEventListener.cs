using UnityEngine;
using UnityEngine.Events;

namespace lisandroct.EventSystem
{
    public interface IGameEventListener { void OnEventRaised(); }
    public interface IGameEventListener<T> { void OnEventRaised(T element); }
    public interface IGameEventListener<T, U> { void OnEventRaised(T element0, U element1); }
    public interface IGameEventListener<T, U, V> { void OnEventRaised(T element0, U element1, V element2); }
    public interface IGameEventListener<T, U, V, W> { void OnEventRaised(T element0, U element1, V element2, W element3); }

    public abstract class GameEventListener<U, V> : MonoBehaviour, IGameEventListener where U : GameEvent where V : UnityEvent
    {
        [SerializeField]
        private U m_Event;
        private U gameEvent { get { return m_Event; } }

        [SerializeField]
        private V m_Response;
        private V response { get { return m_Response; } }

        private void OnEnable() {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable() {
            gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised() {
            response.Invoke();
        }
    }

    public abstract class GameEventListener<T, U, V> : MonoBehaviour, IGameEventListener<T> where U : GameEvent<T> where V : UnityEvent<T>
    {
        [SerializeField]
        private U m_Event;
        private U gameEvent { get { return m_Event; } }

        [SerializeField]
        private V m_Response;
        private V response { get { return m_Response; } }

        private void OnEnable() {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable() {
            gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(T element) {
            response.Invoke(element);
        }
    }

    public abstract class GameEventListener<T0, T1, U, V> : MonoBehaviour, IGameEventListener<T0, T1> where U : GameEvent<T0, T1> where V : UnityEvent<T0, T1>
    {
        [SerializeField]
        private U m_Event;
        private U gameEvent { get { return m_Event; } }

        [SerializeField]
        private V m_Response;
        private V response { get { return m_Response; } }

        private void OnEnable() {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable() {
            gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(T0 element0, T1 element1) {
            response.Invoke(element0, element1);
        }
    }

    public abstract class GameEventListener<T0, T1, T2, U, V> : MonoBehaviour, IGameEventListener<T0, T1, T2> where U : GameEvent<T0, T1, T2> where V : UnityEvent<T0, T1, T2>
    {
        [SerializeField]
        private U m_Event;
        private U gameEvent { get { return m_Event; } }

        [SerializeField]
        private V m_Response;
        private V response { get { return m_Response; } }

        private void OnEnable() {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable() {
            gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(T0 element0, T1 element1, T2 element2) {
            response.Invoke(element0, element1, element2);
        }
    }

    public abstract class GameEventListener<T0, T1, T2, T3, U, V> : MonoBehaviour, IGameEventListener<T0, T1, T2, T3> where U : GameEvent<T0, T1, T2, T3> where V : UnityEvent<T0, T1, T2, T3>
    {
        [SerializeField]
        private U m_Event;
        private U gameEvent { get { return m_Event; } }

        [SerializeField]
        private V m_Response;
        private V response { get { return m_Response; } }

        private void OnEnable() {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable() {
            gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(T0 element0, T1 element1, T2 element2, T3 element3) {
            response.Invoke(element0, element1, element2, element3);
        }
    }
}