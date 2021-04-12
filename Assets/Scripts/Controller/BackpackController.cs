using System.Collections;
using System.Collections.Generic;
using Tiyago1.EventManager;
using UnityEngine;
using System.Linq;

public class BackpackController : MonoBehaviour
{
    public bool HaveBricks => brickCount > 0;

    [SerializeField] private List<Transform> slotPlaceHolders;
    private Dictionary<int, List<Brick>> slots;
    private Vector2 currentPoint;
    private int brickCount => slots.Sum((brick) => brick.Value.Count());

    public void Initialize()
    {
        slots = new Dictionary<int, List<Brick>>();
        for (int i = 0; i < slotPlaceHolders.Count; i++)
        {
            slots.Add(i, new List<Brick>());
        }
        currentPoint = new Vector2(-1, 0);
        EventManager.Instance.OnCollectBrick.Register(() => AddBrick());
        EventManager.Instance.OnReduceBrick.Register(() => RemoveBrick());
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
        brick.transform.localScale = Vector3.one;
        brick.gameObject.SetActive(true);

        slots[(int)currentPoint.x].Add(brick);
    }

    public void RemoveBrick()
    {
        if (!HaveBricks)
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
