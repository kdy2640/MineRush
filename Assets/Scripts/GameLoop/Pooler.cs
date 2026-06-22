using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pooler : MonoBehaviour
{
    [SerializeField] private Poolable prefab;
    Queue<Poolable> pool;
    GameObject poolContainer;

    private void Awake()
    {
        pool = new Queue<Poolable>();
        poolContainer = new GameObject("PoolContainer");
        poolContainer.transform.parent = null;
    }
    public int GetPoolCount()
    {
        return poolContainer.transform.childCount;
    }
    public void ClearAll()
    {
        while (pool.Count > 0)
        {
            Poolable item = pool.Dequeue();
            Destroy(item.gameObject);
        }
    }
    public Poolable Get(object args)
    {
        if (pool.Count > 0)
        {
            Poolable item = pool.Dequeue();
            item.gameObject.SetActive(true);
            item.Initialize(args);
            return item;
        }
        else
        {
            Poolable newItem = Instantiate(prefab, poolContainer.transform);
            newItem.SubscribeReturnListener(Return);
            newItem.Initialize(args);
            return newItem;
        }
    }

    public void Return(Poolable item)
    {
        item.ResetState();
        item.gameObject.SetActive(false);
        pool.Enqueue(item);
    }

}
