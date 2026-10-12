using UnityEngine;

public class ExtractionBeacon : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] availableSystems;
    [SerializeField] private ParticleSystem[] calledSystems;

    public void ShowAvailable()
    {
        foreach (var system in availableSystems)
            system.Play(false);
    }

    public void ShowCalled()
    {
        foreach (var system in calledSystems)
            system.Play(false);
    }
}
