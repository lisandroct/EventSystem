using lisandroct.EventSystem.Events;
using UnityEngine;
using UnityEngine.UI;

namespace lisandroct.EventSystem.Example
{
    public class LightSwitch : MonoBehaviour
    {
        [SerializeField]
        private BoolEvent _onLightToggle;
        private BoolEvent OnLightToggle => _onLightToggle;

        [Space]
        
        [SerializeField]
        private bool _initialState;
        private bool InitialState => _initialState;

        private bool State { get; set; }
        
        private void Start()
        {
            SetState(InitialState);
        }

        public void Toggle()
        {
            SetState(!State);
        }

        private void SetState(bool state)
        {
            State = state;

            OnLightToggle.Invoke(state);
        }
    }
}