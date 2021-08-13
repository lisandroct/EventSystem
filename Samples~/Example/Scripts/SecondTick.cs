using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lisandroct.EventSystem.Example
{
    public class SecondTick : MonoBehaviour
    {
        [SerializeField]
        private GameEvent onTick;
        private GameEvent OnTick => onTick;

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