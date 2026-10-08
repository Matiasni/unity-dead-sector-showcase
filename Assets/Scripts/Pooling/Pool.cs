using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : PoolableObject
{
    private readonly T prefab;
    private readonly Transform container;
    private readonly Stack<T> available = new();

    public Pool(T prefab, Transform container, int prewarmCount = 0)
    {
        this.prefab = prefab;
        this.container = container;

        Prewarm(prewarmCount);
    }

    public void Prewarm(int count)
    {
        for (int i = 0; i < count; i++)
            available.Push(Create());
    }

    public T Get(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        T instance = Take();

        instance.transform.SetParent(parent != null ? parent : container, false);
        instance.transform.SetPositionAndRotation(position, rotation);

        Activate(instance);
        return instance;
    }

    public T Get(Transform parent)
    {
        T instance = Take();

        instance.transform.SetParent(parent, false);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localRotation = Quaternion.identity;

        Activate(instance);
        return instance;
    }

    public void Release(T instance)
    {
        if (!instance.IsSpawned) return;

        instance.MarkSpawned(false);
        instance.OnDespawned();

        instance.gameObject.SetActive(false);
        instance.transform.SetParent(container, false);

        available.Push(instance);
    }

    private T Take()
    {
        while (available.Count > 0)
        {
            T candidate = available.Pop();

            if (candidate != null)
                return candidate;
        }

        return Create();
    }

    private void Activate(T instance)
    {
        instance.MarkSpawned(true);
        instance.gameObject.SetActive(true);
        instance.OnSpawned();
    }

    private T Create()
    {
        T instance = Object.Instantiate(prefab, container);
        instance.name = prefab.name;
        instance.BindPool(pooled => Release((T)pooled));
        instance.gameObject.SetActive(false);
        return instance;
    }
}
