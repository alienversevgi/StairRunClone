using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPooling;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private GameObject stair;
    public Pool<Stair> StairPool { get; private set; }

    public void Initialize()
    {
        StairPool = new Pool<Stair>(new PrefabFactory<Stair>(stair, "Stair"), 20);
    }
}

