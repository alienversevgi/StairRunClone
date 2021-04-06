using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackController : MonoBehaviour
{
    public int BrickCount = int.MaxValue;
    [SerializeField] private List<Transform> slotPlaceHolders;
    [SerializeField] private Dictionary<int, List<Brick>> slots;
    private Vector2 currentPoint;

    public void Initialize()
    {
        slots = new Dictionary<int, List<Brick>>();
        for (int i = 0; i < slotPlaceHolders.Count; i++)
        {
            slots.Add(i, new List<Brick>());
        }
        currentPoint = new Vector2(-1, 0);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AddBrick();
        }

        if (Input.GetKey(KeyCode.Q))
        {
            RemoveBrick();
        }
    }

    public void AddBrick()
    {
        currentPoint.x++;
        if (currentPoint.x >= slots.Count)
        {
            currentPoint.x = 0;
            currentPoint.y++;
        }

        Brick brick = PoolManager.Instance.BrickPool.Allocate();
        brick.transform.SetParent(slotPlaceHolders[(int)currentPoint.x]);
        brick.transform.localPosition = new Vector3(0, currentPoint.y);
        brick.transform.localEulerAngles = Vector3.zero;
        brick.gameObject.SetActive(true);

        slots[(int)currentPoint.x].Add(brick);
    }

    public void RemoveBrick()
    {
        if (currentPoint.y == -1 || currentPoint.y < 0 && currentPoint.x < 0)
            return;

        Brick brick = slots[(int)currentPoint.x][(int)currentPoint.y];
        PoolManager.Instance.BrickPool.Release(brick);
        slots[(int)currentPoint.x].RemoveAt((int)currentPoint.y);

        currentPoint.x--;
        if (currentPoint.x < 0)
        {
            currentPoint.x = slots.Count - 1;
            currentPoint.y--;
        }
    }
}
