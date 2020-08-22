using lisandroct.EventSystem.Events;
using UnityEngine;

namespace lisandroct.EventSystem.Example
{
    public class RandomColorDispatcher : MonoBehaviour
    {
        [SerializeField]
        private ColorEvent onNewColor;
        private ColorEvent OnNewColor => onNewColor;
        
        public void DispatchRandomColor()
        {
            OnNewColor.Invoke(new Color(Random.value, Random.value, Random.value, 1));
        }
    }
}