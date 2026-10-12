using UnityEngine;

public class ExplosionEffect : PooledEffect
{
    [SerializeField] private float referenceRadius = 4f;

    public void Play(float radius)
    {
        SetScale(radius / referenceRadius);
    }
}
