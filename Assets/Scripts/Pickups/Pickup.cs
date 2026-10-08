using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Pickup : MonoBehaviour
{
    [SerializeField, Tooltip("Seconds to reappear after being collected (0 = never)")]
    private float respawnDelay = 15f;
    [SerializeField] private SoundEffect collectSound;

    private Collider trigger;
    private Renderer[] renderers;

    private void Awake()
    {
        trigger = GetComponent<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!TryApply(other)) return;

        SoundPlayer.Play(collectSound, transform.position);

        SetAvailable(false);

        if (respawnDelay > 0f)
            StartCoroutine(RespawnAfterDelay());
    }

    protected abstract bool TryApply(Collider other);

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        SetAvailable(true);
    }

    private void SetAvailable(bool available)
    {
        trigger.enabled = available;

        foreach (var pickupRenderer in renderers)
            pickupRenderer.enabled = available;
    }
}
