using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AimLaser : MonoBehaviour
{
    [SerializeField] private CharacterCombatState combat;
    [SerializeField] private float maxDistance = 25f;
    [SerializeField] private LayerMask obstacleMask = ~0;

    private LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
    }

    private void LateUpdate()
    {
        line.enabled = combat.IsAiming;

        if (!line.enabled) return;

        Vector3 start = transform.position;
        Vector3 end = start + transform.forward * maxDistance;

        if (Physics.Raycast(start, transform.forward, out RaycastHit hit, maxDistance, obstacleMask, QueryTriggerInteraction.Ignore))
            end = hit.point;

        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }

    private void OnDisable()
    {
        line.enabled = false;
    }
}
