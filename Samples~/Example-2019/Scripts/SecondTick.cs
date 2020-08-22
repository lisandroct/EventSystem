using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event = lisandroct.EventSystem.Event;

namespace lisandroct.EventSystem.Example
{
    public class SecondTick : MonoBehaviour
    {
        [SerializeField]
        private Event onTick;
        private Event OnTick => onTick;

        private IEnumerator Start()
        {
            do
            {
                yield return new WaitForSeconds(1f);

                OnTick.Invoke();
            } while (true);
        }
    }
}