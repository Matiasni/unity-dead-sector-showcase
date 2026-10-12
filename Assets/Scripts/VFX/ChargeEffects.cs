using UnityEngine;

public class ChargeEffects : MonoBehaviour, IChargeFeedback
{
    [SerializeField] private ParticleSystem[] windupSystems;
    [SerializeField] private ParticleSystem[] chargeSystems;
    [SerializeField] private PooledEffect impactEffect;
    [SerializeField] private LayerMask groundMask = 1;

    private void OnDisable()
    {
        Stop();
    }

    public void ShowWindup()
    {
        SetPlaying(chargeSystems, false);
        SetPlaying(windupSystems, true);
    }

    public void ShowCharge()
    {
        SetPlaying(windupSystems, false);
        SetPlaying(chargeSystems, true);
    }

    public void ShowImpact(Vector3 point)
    {
        if (impactEffect == null) return;

        if (Physics.Raycast(point + Vector3.up, Vector3.down, out RaycastHit hit, 4f, groundMask, QueryTriggerInteraction.Ignore))
            point = hit.point;

        PoolManager.Spawn(impactEffect, point, Quaternion.identity);
    }

    public void Stop()
    {
        SetPlaying(windupSystems, false);
        SetPlaying(chargeSystems, false);
    }

    private static void SetPlaying(ParticleSystem[] systems, bool playing)
    {
        foreach (var system in systems)
        {
            if (playing)
                system.Play(false);
            else
                system.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}
