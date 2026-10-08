using System;
using UnityEngine;

public abstract class PoolableObject : MonoBehaviour
{
    private Action<PoolableObject> releaseAction;

    public bool IsSpawned { get; private set; }

    public void BindPool(Action<PoolableObject> releaseAction)
    {
        this.releaseAction = releaseAction;
    }

    public void MarkSpawned(bool spawned)
    {
        IsSpawned = spawned;
    }

    public void Release()
    {
        if (!IsSpawned) return;

        if (releaseAction != null)
            releaseAction(this);
        else
            Destroy(gameObject);
    }

    public virtual void OnSpawned() { }

    public virtual void OnDespawned() { }
}
