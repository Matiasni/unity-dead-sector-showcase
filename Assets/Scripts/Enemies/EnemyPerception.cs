using UnityEngine;

public class EnemyPerception
{
    private readonly TargetScanner scanner;
    private readonly float scanInterval;
    private readonly float aggroRadius;
    private readonly float alertRadius;
    private readonly LayerMask targetMask;

    private float nextScanTime;

    public Transform Target { get; private set; }
    public bool HasTarget => Target != null && Target.gameObject.activeInHierarchy;
    public bool IsAlerted { get; private set; }

    public EnemyPerception(float aggroRadius, float alertRadius, LayerMask targetMask, float scanInterval)
    {
        this.aggroRadius = aggroRadius;
        this.alertRadius = alertRadius;
        this.targetMask = targetMask;
        this.scanInterval = scanInterval;

        scanner = new TargetScanner(aggroRadius, targetMask);
    }

    public void Tick(Vector3 origin)
    {
        if (Time.time < nextScanTime) return;

        nextScanTime = Time.time + scanInterval;

        var closest = scanner.FindClosest(origin);
        Target = closest != null ? closest.transform : null;

        if (Target != null)
            Alert();
    }

    public void Alert()
    {
        if (IsAlerted) return;

        IsAlerted = true;
        scanner.Configure(alertRadius, targetMask);
        nextScanTime = 0f;
    }

    public void Clear()
    {
        Target = null;
        IsAlerted = false;
        nextScanTime = 0f;
        scanner.Configure(aggroRadius, targetMask);
    }
}
