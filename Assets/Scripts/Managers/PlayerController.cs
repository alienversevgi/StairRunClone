using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using System;
using UnityEngine.Animations.Rigging;
using Tiyago1.EventManager;

public class PlayerController : MonoBehaviour
{
    private const float STAIR_STEP_SIZE = 0.5f;

    [SerializeField] private float stairInstantiateRate;
    [SerializeField] private float speed;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private BackpackController backpack;
    [SerializeField] private Rig backpackRig;

    private Vector3 direction;
    private bool isEnable;
    private Rigidbody rigidbody;
    private Animator animator;

    private Coroutine climpStair;

    #region Unity Methods

    private void Update()
    {
        if (isEnable)
        {
            if (backpack.HaveBricks)
            { 
                if (Input.GetMouseButtonDown(0))
                {
                    Run();
                }

                if (Input.GetMouseButtonUp(0))
                {
                    Glide();
                }
            }

            this.transform.position += (this.transform.forward + direction) * speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isEnable && collision.collider.CompareTag("Ground"))
        {
            SetDirection(Vector3.zero);
            animator.SetTrigger("Run");
            backpackRig.weight = 1;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        Brick brick = collider.gameObject.GetComponent<Brick>();
        if (brick != null)
        {
            brick.Release();
            EventManager.Instance.OnCollectBrick.Raise();
        }
    }

    #endregion

    #region Public Methods

    public void Initialize()
    {
        backpack.Initialize();
        rigidbody = this.GetComponent<Rigidbody>();
        animator = this.GetComponent<Animator>();
        SetDirection(Vector3.zero);
    }

    public void SetEnable(bool isEnable)
    {
        this.isEnable = isEnable;
    }

    public void Push(Vector3 direction, float time)
    {
        SetDirection(direction);
        Timer.Instance.StartTimer(time, () =>
            {
                Glide();
            }
        );
    }

    #endregion

    #region Private Methods

    private void Glide()
    {
        SetDirection(new Vector3(0, -.5f, 0));
        animator.SetTrigger("Glide");
        backpackRig.weight = 1;
        if (climpStair != null)
            StopCoroutine(climpStair);
        PoolManager.Instance.StairPool.unavailable.ToList().ForEach(it => it.Release());
        SetRigidbodyProperties(false);
    }

    private void Run()
    {
        SetDirection(Vector3.zero);
        animator.SetTrigger("Run");
        backpackRig.weight = 1;
        climpStair = StartCoroutine(ClimpStair());
        SetRigidbodyProperties(true);
    }

    private void SetRigidbodyProperties(bool isRunning)
    {
        rigidbody.useGravity = !isRunning;
        rigidbody.isKinematic = isRunning;
    }

    private void CreateStair()
    {
        Stair stair = PoolManager.Instance.StairPool.Allocate();
        stair.transform.position = spawnPoint.position;
        stair.transform.localEulerAngles = this.transform.eulerAngles;
        stair.gameObject.SetActive(true);
        stair.SetReleaseAction(() => PoolManager.Instance.StairPool.Release(stair));
        stair.Initialize();
    }

    private void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    private IEnumerator ClimpStair()
    {
        while (backpack.HaveBricks)
        {
            SetDirection(Vector3.zero);
            this.transform.DOMoveY(this.transform.position.y + STAIR_STEP_SIZE, .2f);
            backpackRig.weight = 0;
            EventManager.Instance.OnReduceBrick.Raise();
            CreateStair();
            yield return new WaitForSecondsRealtime(stairInstantiateRate);
        }
        Glide();
    }

    #endregion
}