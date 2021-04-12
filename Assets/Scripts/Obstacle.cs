using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Vector3 forceDirection;
    [SerializeField] private float time = .5f;
    private void OnCollisionEnter(Collision collision)
    {
        PlayerController playerController = collision.collider.GetComponent<PlayerController>();

        if (playerController != null)
            playerController.Push( -2 * playerController.transform.forward + forceDirection, time);
    }
}
