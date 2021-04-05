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
    [SerializeField] private BackpackController backpack;
    [SerializeField] private Rig backpackRig;

    private bool isEnable;
    private Rigidbody rigidbody;
    private Animator animator;

    private Coroutine climpStair;

    #region Unity Methods

    private void Update()
    {
        if (isEnable)
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isEnable && collision.collider.CompareTag("Ground"))
        {
            animator.SetTrigger("Run");
            backpackRig.weight = 1;
        }
    }

    #endregion

    #region Public Methods

    public void Initialize()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        animator = this.GetComponent<Animator>();
    }

    public void SetEnable(bool isEnable)
    {
        this.isEnable = isEnable;
    }

    #endregion

    #region Private Methods

    private void Glide()
    {
        animator.SetTrigger("Glide");
        OnGliding?.Invoke();
        PoolManager.Instance.StairPool.unavailable.ToList().ForEach(it => it.Release());
        backpackRig.weight = 1;
        StopCoroutine(climpStair);
        SetRigidbodyProperties(false);
    }

    private void Run()
    {
        animator.SetTrigger("Run");
        backpackRig.weight = 1;
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
        while (backpack.BrickCount > 0)
        {
            this.transform.position = this.transform.position + new Vector3(0, STAIR_STEP_SIZE, 0);
            backpackRig.weight = 0;
            //this.transform.DOMoveY(this.transform.position.y + STAIR_STEP_SIZE, .1f);
            OnStepCompleted?.Invoke(spawnPoint.position);
            CreateStair(spawnPoint.position);
            yield return new WaitForSecondsRealtime(stairInstantiateRate);
        }
    }


    private void CreateStair(Vector3 spawnPosition)
    {
        Stair stair = PoolManager.Instance.StairPool.Allocate();
        stair.transform.position = spawnPosition;
        stair.gameObject.SetActive(true);
        stair.SetReleaseAction(() => PoolManager.Instance.StairPool.Release(stair));
        stair.Initialize();
    }

    #endregion
}