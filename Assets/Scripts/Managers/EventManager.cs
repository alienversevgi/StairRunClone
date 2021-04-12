using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tiyago1.EventManager
{
    public class EventManager : MonoBehaviour
    {
        #region Singelaton

        private static EventManager instance;

        public static EventManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<EventManager>();
                }

                return instance;
            }
        }
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        #endregion

        public Event OnCollectBrick;
        public Event OnReduceBrick;
    }
}
