using System;
using System.Collections;
using UnityEngine;

public class ReinforcementSystem : MonoBehaviour
{
    [SerializeField] private HealthBehaviour playerHealth;
    [SerializeField] private Transform defaultRespawnPoint;
    [SerializeField] private DropPod dropPodPrefab;
    [SerializeField] private float respawnDelay = 2f;

    private Transform respawnPoint;
    private Rigidbody playerBody;

    public int Remaining { get; private set; }

    public event Action<int> OnReinforcementsChanged;
    public event Action OnDepleted;

    private void Awake()
    {
        respawnPoint = defaultRespawnPoint;
        playerBody = playerHealth.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        playerHealth.OnDied += HandlePlayerDied;
    }

    private void OnDisable()
    {
        playerHealth.OnDied -= HandlePlayerDied;
    }

    public void Setup(int reinforcements)
    {
        Remaining = reinforcements;
        OnReinforcementsChanged?.Invoke(Remaining);
    }

    public void SetCheckpoint(Transform point)
    {
        respawnPoint = point;
    }

    private void HandlePlayerDied()
    {
        if (Remaining <= 0)
        {
            OnDepleted?.Invoke();
            return;
        }

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);

        Remaining--;
        OnReinforcementsChanged?.Invoke(Remaining);

        Vector3 position = respawnPoint.position;
        var pod = PoolManager.Spawn(dropPodPrefab, position, Quaternion.identity);

        yield return new WaitForSeconds(pod.DropDuration);

        PlacePlayer(position);
        playerHealth.ResetHealth();
    }

    private void PlacePlayer(Vector3 position)
    {
        if (playerBody != null)
        {
            playerBody.linearVelocity = Vector3.zero;
            playerBody.position = position;
        }

        playerHealth.transform.position = position;
    }
}
