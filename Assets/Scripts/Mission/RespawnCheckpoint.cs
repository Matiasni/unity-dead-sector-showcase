using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RespawnCheckpoint : MonoBehaviour
{
    [SerializeField] private ReinforcementSystem reinforcements;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private LayerMask playerMask;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((playerMask.value & (1 << other.gameObject.layer)) == 0) return;

        reinforcements.SetCheckpoint(respawnPoint != null ? respawnPoint : transform);
    }
}
