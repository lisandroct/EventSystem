using System.Collections;
using System.Collections.Generic;
using lisandroct.EventSystem.Events;
using UnityEngine;
using UnityEngine.UI;

namespace lisandroct.EventSystem.Example
{
    public class Switch : MonoBehaviour
    {
        [SerializeField]
        private Slider _slider;
        private Slider Slider => _slider;
        
        public void SetState(bool state) => Slider.value = state ? 1 : 0;
    }
}