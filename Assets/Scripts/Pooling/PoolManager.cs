using System.Collections.Generic;
using UnityEngine;

public static class PoolManager
{
    private static readonly Dictionary<PoolableObject, Pool<PoolableObject>> pools = new();
    private static Transform root;

    public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null) where T : PoolableObject
    {
        return (T)GetPool(prefab).Get(position, rotation, parent);
    }

    public static T Spawn<T>(T prefab, Transform parent) where T : PoolableObject
    {
        return (T)GetPool(prefab).Get(parent);
    }

    public static void Prewarm(PoolableObject prefab, int count)
    {
        GetPool(prefab).Prewarm(count);
    }

    private static Pool<PoolableObject> GetPool(PoolableObject prefab)
    {
        EnsureRoot();

        if (!pools.TryGetValue(prefab, out var pool))
        {
            var container = new GameObject($"Pool_{prefab.name}").transform;
            container.SetParent(root, false);

            pool = new Pool<PoolableObject>(prefab, container);
            pools.Add(prefab, pool);
        }

        return pool;
    }

    private static void EnsureRoot()
    {
        if (root != null) return;

        pools.Clear();
        root = new GameObject("[Pools]").transform;
    }
}
