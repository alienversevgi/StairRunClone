using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPooling;
using System;

public class Brick : MonoBehaviour, IPoolElement
{
    public void Reset()
    {
        this.gameObject.SetActive(false);
    }

    public void SetReleaseAction(Action releaseAction)
    {
        
    }
}
