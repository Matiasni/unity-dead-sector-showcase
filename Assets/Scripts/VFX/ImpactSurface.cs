using System;
using UnityEngine;

public class ImpactSurface : MonoBehaviour
{
    [SerializeField] private PooledEffect impactEffect;

    public PooledEffect ImpactEffect => impactEffect;

    public event Action<Vector3> OnImpact;

    public void NotifyImpact(Vector3 point)
    {
        OnImpact?.Invoke(point);
    }
}
