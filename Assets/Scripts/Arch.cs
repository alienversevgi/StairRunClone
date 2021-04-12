using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class Arch : MonoBehaviour
{
    [SerializeField] private Vector3 rotation;
    [SerializeField] private float time = 2.4f;

    private void OnTriggerEnter(Collider collider)
    {
        PlayerController playerController = collider.GetComponent<PlayerController>();

        if (playerController != null)
        {
            playerController.transform.DORotate(rotation, time);
        }
    }
}
