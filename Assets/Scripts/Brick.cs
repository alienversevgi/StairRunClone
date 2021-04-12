using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPooling;
using System;

public class Brick : MonoBehaviour, IPoolElement
{
    private Action releaseAction;
    public void Reset()
    {
        this.gameObject.SetActive(false);
    }

    public void SetReleaseAction(Action releaseAction)
    {
        this.releaseAction = releaseAction;
    }

    public void Release()
    {
        this.gameObject.SetActive(false);
    }
}
