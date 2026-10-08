using UnityEngine;

[RequireComponent(typeof(HealthBehaviour))]
public class DropRewardOnDeath : MonoBehaviour
{
    [SerializeField] private RewardDefinition reward;
    [SerializeField] private LootDrop dropPrefab;
    [SerializeField] private float dropHeight = 0.5f;

    private HealthBehaviour health;

    private void Awake()
    {
        health = GetComponent<HealthBehaviour>();
    }

    private void OnEnable()
    {
        health.OnDied += Drop;
    }

    private void OnDisable()
    {
        health.OnDied -= Drop;
    }

    private void Drop()
    {
        Vector3 position = transform.position;
        position.y = dropHeight;

        var drop = PoolManager.Spawn(dropPrefab, position, Quaternion.identity);
        drop.Setup(reward);
    }
}
