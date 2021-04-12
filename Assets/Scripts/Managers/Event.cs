using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tiyago1.EventManager
{
    [CreateAssetMenu()]
    public class Event : ScriptableObject
    {
        public event Action EventListeners;

        public void Raise()
        {
            EventListeners.Invoke();
        }

        public void Register(Action listener)
        {
            EventListeners += listener;
        }

        public void UnregisterListener(Action listener)
        {
            EventListeners -= listener;
        }
    }
}