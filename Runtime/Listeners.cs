using UnityEngine;
using UnityEngine.Events;

namespace lisandroct.EventSystem
{
    public interface IListener<in T0> { void OnEventRaised(T0 arg); }
    public interface IListener<in T0, in T1> { void OnEventRaised(T0 arg0, T1 arg1); }
    public interface IListener<in T0, in T1, in T2> { void OnEventRaised(T0 arg0, T1 arg1, T2 arg2); }
    public interface IListener<in T0, in T1, in T2, in T3> { void OnEventRaised(T0 arg0, T1 arg1, T2 arg2, T3 arg3); }
    
    #if UNITY_2020_1_OR_NEWER
    public abstract class Listener<T> : MonoBehaviour, IListener<T> 
    {
        [SerializeField]
        private GameEvent<T> _event;
        private GameEvent<T> Event => _event;

        [SerializeField]
        private UnityEvent<T> _response;
        private UnityEvent<T> Response => _response;

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

        public void OnEventRaised(T arg) => Response?.Invoke(arg);
    }

    public abstract class Listener<T0, T1> : MonoBehaviour, IListener<T0, T1>
    {
        [SerializeField]
        private GameEvent<T0, T1> _event;
        private GameEvent<T0, T1> Event => _event;

        [SerializeField]
        private UnityEvent<T0, T1> _response;
        private UnityEvent<T0, T1> Response => _response;

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

        public void OnEventRaised(T0 arg0, T1 arg1) => Response?.Invoke(arg0, arg1);
    }

    public abstract class Listener<T0, T1, T2> : MonoBehaviour, IListener<T0, T1, T2>
    {
        [SerializeField]
        private GameEvent<T0, T1, T2> _event;
        private GameEvent<T0, T1, T2> Event => _event;

        [SerializeField]
        private UnityEvent<T0, T1, T2> _response;
        private UnityEvent<T0, T1, T2> Response => _response;

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

        public void OnEventRaised(T0 arg0, T1 arg1, T2 arg2) => Response?.Invoke(arg0, arg1, arg2);
    }

    public abstract class Listener<T0, T1, T2, T3> : MonoBehaviour, IListener<T0, T1, T2, T3>
    {
        [SerializeField]
        private GameEvent<T0, T1, T2, T3> _event;
        private GameEvent<T0, T1, T2, T3> Event => _event;

        [SerializeField]
        private UnityEvent<T0, T1, T2, T3> _response;
        private UnityEvent<T0, T1, T2, T3> Response => _response;

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

        public void OnEventRaised(T0 arg0, T1 arg1, T2 arg2, T3 arg3) => Response?.Invoke(arg0, arg1, arg2, arg3);
    }
    #else
    public abstract class Listener<T, E, R> : MonoBehaviour, IListener<T> where E : GameEvent<T> where R : UnityEvent<T>
    {
        [SerializeField]
        private E _event;
        private E Event => _event;

        [SerializeField]
        private R _response;
        private R response => _response;

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

        public void OnEventRaised(T arg) => response?.Invoke(arg);
    }

    public abstract class Listener<T0, T1, E, R> : MonoBehaviour, IListener<T0, T1> where E : GameEvent<T0, T1> where R : UnityEvent<T0, T1>
    {
        [SerializeField]
        private E _event;
        private E Event => _event;

        [SerializeField]
        private R _response;
        private R Response => _response;

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

        public void OnEventRaised(T0 arg0, T1 arg1) => Response?.Invoke(arg0, arg1);
    }

    public abstract class Listener<T0, T1, T2, E, R> : MonoBehaviour, IListener<T0, T1, T2> where E : GameEvent<T0, T1, T2> where R : UnityEvent<T0, T1, T2>
    {
        [SerializeField]
        private E _event;
        private E Event => _event;

        [SerializeField]
        private R _response;
        private R Response => _response;

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

        public void OnEventRaised(T0 arg0, T1 arg1, T2 arg2) => Response?.Invoke(arg0, arg1, arg2);
    }

    public abstract class Listener<T0, T1, T2, T3, E, R> : MonoBehaviour, IListener<T0, T1, T2, T3> where E : GameEvent<T0, T1, T2, T3> where R : UnityEvent<T0, T1, T2, T3>
    {
        [SerializeField]
        private E _event;
        private E Event => _event;

        [SerializeField]
        private R _response;
        private R Response => _response;

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

        public void OnEventRaised(T0 arg0, T1 arg1, T2 arg2, T3 arg3) => Response?.Invoke(arg0, arg1, arg2, arg3);
    }
    #endif
}