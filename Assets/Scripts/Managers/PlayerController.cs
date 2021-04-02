using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using System;
using UnityEngine.Animations.Rigging;

public class PlayerController : MonoBehaviour
{
    private const float STAIR_STEP_SIZE = 0.5f;

    public event Action OnRunning;
    public event Action OnGliding;
    public event Action<Vector3> OnStepCompleted;

    [SerializeField] private float stairInstantiateRate;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Rig chestRig;

    private Rigidbody rigidbody;
    private Animator animator;

    private Coroutine climpStair;

    public void Initialize()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        animator = this.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Run();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Glide();
        }

        this.transform.position += this.transform.forward * 10 * Time.deltaTime;
    }

    private void Glide()
    {
        animator.SetTrigger("Glide");
        OnGliding?.Invoke();
        chestRig.weight = 1;
        StopCoroutine(climpStair);
        SetRigidbodyProperties(false);
    }

    private void Run()
    {
        animator.SetTrigger("Run");
        chestRig.weight = 1;
        OnRunning?.Invoke();
        climpStair = StartCoroutine(ClimpStair());
        SetRigidbodyProperties(true);
    }

    private void SetRigidbodyProperties(bool isRunning)
    {
        rigidbody.useGravity = !isRunning;
        rigidbody.isKinematic = isRunning;
    }

    private IEnumerator ClimpStair()
    {
        while (true) // stair count
        {
            yield return new WaitForSecondsRealtime(stairInstantiateRate);
            this.transform.position = this.transform.position + new Vector3(0, STAIR_STEP_SIZE, 0);
            chestRig.weight = 0;
            //this.transform.DOMoveY(this.transform.position.y + STAIR_STEP_SIZE, .1f);
            OnStepCompleted?.Invoke(spawnPoint.position);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            animator.SetTrigger("Run");
            chestRig.weight = 1;
        }
    }
}
