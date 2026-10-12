using UnityEngine;

public class PooledEffect : PoolableObject
{
    [SerializeField] private float lifetime = 1f;

    private ParticleSystem[] systems;
    private float releaseTime;

    protected virtual void Awake()
    {
        systems = GetComponentsInChildren<ParticleSystem>(true);
    }

    public override void OnSpawned()
    {
        transform.localScale = Vector3.one;
        releaseTime = Time.time + lifetime;

        foreach (var system in systems)
        {
            system.Clear(false);
            system.Play(false);
        }
    }

    public void SetTint(Color color)
    {
        foreach (var system in systems)
        {
            var main = system.main;
            main.startColor = color;
        }
    }

    public void SetScale(float scale)
    {
        transform.localScale = Vector3.one * scale;
    }

    private void Update()
    {
        if (lifetime > 0f && Time.time >= releaseTime)
            Release();
    }
}
