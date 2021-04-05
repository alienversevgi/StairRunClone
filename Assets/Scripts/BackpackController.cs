using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackController : MonoBehaviour
{
    public int BrickCount = int.MaxValue;
    [SerializeField] private List<Transform> slotPlaceHolders;
    [SerializeField]private Dictionary<int, List<bool>> slots;
    private int currentIndex;
    private Vector2 cur;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        slots = new Dictionary<int, List<bool>>();
        for (int i = 0; i < slotPlaceHolders.Count; i++)
        {
            slots.Add(i, new List<bool>(50));
        }
        cur = new Vector2(-1, 0);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            AddBrick();
            Brick brick = PoolManager.Instance.BrickPool.Allocate();
            brick.transform.SetParent(slotPlaceHolders[(int)cur.x]);
            brick.transform.localPosition = new Vector3(0, cur.y);
            brick.gameObject.SetActive(true);
        }
    }

    public void AddBrick()
    {
        cur.x++;
        if (cur.x >= slots.Count)
        {
            cur.x = 0;
            cur.y++;
        }

        if (cur.y <= slots[(int)cur.x].Count)
        {
            slots[(int)cur.x].Add(false);
        }

        slots[(int)cur.x][(int)cur.y] = true;
    }
}
