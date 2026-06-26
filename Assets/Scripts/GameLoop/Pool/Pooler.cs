using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PoolArgs 
{

}

public class Pooler<T> : MonoBehaviour where T : Poolable
{
    [SerializeField] protected T prefab;

    private readonly Queue<T> pool = new();

    private static GameObject poolContainer;
    private static readonly Dictionary<System.Type, Transform> typeContainers = new();

    private static Transform Container
    {
        get
        {
            if (poolContainer == null)
            {
                poolContainer = new GameObject("[Pool Container]"); 
            }

            return poolContainer.transform;
        }
    }

    private static Transform TypeContainer
    {
        get
        {
            System.Type type = typeof(T);

            if (typeContainers.TryGetValue(type, out Transform cached))
                return cached;

            GameObject go = new GameObject($"[Pool] {type.Name}");
            go.transform.SetParent(Container);

            typeContainers.Add(type, go.transform);
            return go.transform;
        }
    }

    public virtual T Get(PoolArgs args)
    {
        T item = pool.Count > 0 ? pool.Dequeue() : Create();

        item.gameObject.SetActive(true);
        item.Initialize(args);
        return item;
    }

    public void Prewarm(int count)
    {
        if (count <= 0)
            return;

        for (int i = 0; i < count; i++)
        {
            T item = Create();
            item.gameObject.SetActive(false);
            pool.Enqueue(item);
        }
    }
    private T Create()
    {
        T item = Instantiate(prefab, TypeContainer);
        item.SubscribeReturnListener(Return);
        return item;
    }

    private void Return(Poolable item)
    {
        item.ResetState();
        item.gameObject.SetActive(false);
        pool.Enqueue((T)item);
    }
}