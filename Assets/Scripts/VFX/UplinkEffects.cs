using UnityEngine;

public class UplinkEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] activeSystems;
    [SerializeField] private ParticleSystem[] completeSystems;
    [SerializeField] private PooledEffect lostEffect;
    [SerializeField] private Transform lostEffectPoint;

    public void ShowActive()
    {
        foreach (var system in activeSystems)
            system.Play(false);
    }

    public void ShowLost()
    {
        StopActive();

        if (lostEffect != null)
            PoolManager.Spawn(lostEffect, lostEffectPoint.position, Quaternion.identity);
    }

    public void ShowComplete()
    {
        StopActive();

        foreach (var system in completeSystems)
            system.Play(false);
    }

    private void StopActive()
    {
        foreach (var system in activeSystems)
            system.Stop(false, ParticleSystemStopBehavior.StopEmitting);
    }
}
