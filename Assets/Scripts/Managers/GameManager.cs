using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PoolManager poolManager;

    private void Start()
    {
        Initialize();
    }

    #region General Methods

    private void Initialize()
    {
        poolManager.Initialize();
        playerController.Initialize();
        SubscribeEvents();
    }


    public void StartGame()
    {

    }

    public void GameOver()
    {
        UnSubscribeEvents();
        SceneManager.LoadScene(0);
    }

    #endregion

    #region Events

    private void SubscribeEvents()
    {
        playerController.OnRunning += PlayerController_OnRunning;
        playerController.OnGliding += PlayerController_OnGliding;
        playerController.OnStepCompleted += PlayerController_OnStepCompleted;
    }

    private void PlayerController_OnGliding()
    {
        poolManager.StairPool.unavailable.ToList().ForEach(it => it.Release());
    }

    private void PlayerController_OnRunning()
    {

    }

    private void UnSubscribeEvents()
    {
        playerController.OnStepCompleted -= PlayerController_OnStepCompleted;
    }

    private void PlayerController_OnStepCompleted(Vector3 spawnPosition)
    {
        Stair stair = poolManager.StairPool.Allocate();
        stair.transform.position = spawnPosition;
        stair.gameObject.SetActive(true);
        stair.Initialize(() => poolManager.StairPool.Release(stair));
    }

    #endregion
}
