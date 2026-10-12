using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LootDrop : PoolableObject
{
    [SerializeField] private float lifeTime = 30f;
    [SerializeField] private float spinSpeed = 120f;
    [SerializeField] private float bobHeight = 0.15f;
    [SerializeField] private Transform visual;
    [SerializeField] private SoundEffect collectSound;
    [SerializeField] private PickupGlow glow;
    [SerializeField] private PooledEffect collectEffect;

    private RewardDefinition reward;
    private float expireTime;

    public void Setup(RewardDefinition reward)
    {
        this.reward = reward;
        expireTime = Time.time + lifeTime;

        if (glow != null)
            glow.SetColor(reward.DisplayColor);
    }

    public override void OnDespawned()
    {
        reward = null;
    }

    private void Update()
    {
        if (Time.time >= expireTime)
        {
            Release();
            return;
        }

        visual.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.World);
        visual.localPosition = Vector3.up * (Mathf.Sin(Time.time * 3f) * bobHeight);
    }

    private void SpawnCollectEffect(Vector3 collectorPosition)
    {
        if (collectEffect == null) return;

        Vector3 direction = collectorPosition - transform.position;
        direction.y = 0f;

        var rotation = direction.sqrMagnitude > 0.001f ? Quaternion.LookRotation(direction) : Quaternion.identity;
        var effect = PoolManager.Spawn(collectEffect, transform.position, rotation);
        effect.SetTint(reward.DisplayColor);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (reward == null) return;

        var wallet = other.GetComponentInParent<IResourceWallet>();

        if (wallet == null) return;

        SpawnCollectEffect(other.transform.position);

        reward.GrantTo(wallet);
        SoundPlayer.Play(collectSound, transform.position);
        Release();
    }
}
