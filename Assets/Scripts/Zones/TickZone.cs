using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class TickZone<T> : MonoBehaviour where T : class
{
    [SerializeField] private float tickInterval = 0.5f;

    private readonly HashSet<T> targets = new();
    private float nextTickTime;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponentInParent<T>();

        if (target != null)
            targets.Add(target);
    }

    private void OnTriggerExit(Collider other)
    {
        var target = other.GetComponentInParent<T>();

        if (target != null)
            targets.Remove(target);
    }

    private void Update()
    {
        if (Time.time < nextTickTime) return;

        nextTickTime = Time.time + tickInterval;

        targets.RemoveWhere(IsUnavailable);

        foreach (var target in targets)
            Apply(target);
    }

    private static bool IsUnavailable(T target) => target is Behaviour behaviour && (behaviour == null || !behaviour.isActiveAndEnabled);

    protected abstract void Apply(T target);
}
