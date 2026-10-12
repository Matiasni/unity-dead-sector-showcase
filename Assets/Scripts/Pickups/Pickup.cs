using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Pickup : MonoBehaviour
{
    [SerializeField, Tooltip("Seconds to reappear after being collected (0 = never)")]
    private float respawnDelay = 15f;
    [SerializeField] private SoundEffect collectSound;
    [SerializeField] private PooledEffect collectEffect;
    [SerializeField] private Color effectColor = Color.white;

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
        SpawnCollectEffect(other.transform.position);

        SetAvailable(false);

        if (respawnDelay > 0f)
            StartCoroutine(RespawnAfterDelay());
    }

    protected abstract bool TryApply(Collider other);

    private void SpawnCollectEffect(Vector3 collectorPosition)
    {
        if (collectEffect == null) return;

        Vector3 direction = collectorPosition - transform.position;
        direction.y = 0f;

        var rotation = direction.sqrMagnitude > 0.001f ? Quaternion.LookRotation(direction) : Quaternion.identity;
        var effect = PoolManager.Spawn(collectEffect, transform.position, rotation);
        effect.SetTint(effectColor);
    }

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
