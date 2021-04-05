using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPooling;

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField] private GameObject stair;
    [SerializeField] private GameObject brick;
    
    public Pool<Stair> StairPool { get; private set; }
    public Pool<Brick> BrickPool { get; private set; }

    public void Initialize()
    {
        StairPool = new Pool<Stair>(new PrefabFactory<Stair>(stair, "Stair"), 20);
        BrickPool = new Pool<Brick>(new PrefabFactory<Brick>(brick, "Brick"), 20);
    }
}