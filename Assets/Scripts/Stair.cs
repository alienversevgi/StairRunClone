using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPooling;

public class Stair : MonoBehaviour, IPoolElement
{
    private Action onRelease;
    private Rigidbody rigidbody;
    private Action releaseAction;

    public void Reset()
    {
        this.gameObject.SetActive(false);
        this.transform.position = Vector3.zero;
        this.transform.eulerAngles = Vector3.zero;
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    public void SetReleaseAction(Action releaseAction)
    {
        onRelease = releaseAction;
    }

    public void Initialize()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
    }

    public void Release()
    {
        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        StartCoroutine(WaitAndExecute(2f, () => onRelease()));
    }

    private IEnumerator WaitAndExecute(float waitTime, Action action)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        action();
    }
}
