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
        StartGame();
    }

    private void Initialize()
    {
        poolManager.Initialize();
        playerController.Initialize();
    }

    public void StartGame()
    {
        playerController.SetEnable(true);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(0);
    }
}