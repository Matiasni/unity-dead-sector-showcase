using UnityEngine;

public class AutoTurret : MonoBehaviour
{
    [SerializeField] private WeaponSettings weaponSettings;
    [SerializeField] private WeaponView weaponView;
    [SerializeField] private Transform aimPivot;

    [Header("Targeting")]
    [SerializeField] private float detectionRadius = 12f;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private float scanInterval = 0.2f;
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField, Range(0f, 45f)] private float fireAngle = 8f;

    private Weapon weapon;
    private TargetScanner scanner;

    private Collider target;
    private float nextScanTime;

    private void Awake()
    {
        scanner = new TargetScanner(detectionRadius, targetMask);

        if (weaponSettings != null)
            weapon = new Weapon(weaponSettings, weaponView);
    }

    public void Configure(WeaponSettings settings, float radius, LayerMask mask, IWeaponModifiers modifiers = null)
    {
        weaponSettings = settings;
        detectionRadius = radius;
        targetMask = mask;

        scanner.Configure(radius, mask);
        weapon = new Weapon(settings, weaponView, null, modifiers);
    }

    private void OnDisable()
    {
        target = null;
        weapon?.ReleaseTrigger();
    }

    private void Update()
    {
        if (weapon == null) return;

        UpdateTarget();

        if (!HasValidTarget())
        {
            weapon.SetTrigger(false);
            return;
        }

        Vector3 direction = target.bounds.center - aimPivot.position;

        aimPivot.rotation = Quaternion.Slerp(
            aimPivot.rotation,
            Quaternion.LookRotation(direction),
            turnSpeed * Time.deltaTime
        );

        weapon.SetTrigger(Vector3.Angle(aimPivot.forward, direction) <= fireAngle);
    }

    private void UpdateTarget()
    {
        if (Time.time < nextScanTime) return;

        nextScanTime = Time.time + scanInterval;
        target = scanner.FindClosest(aimPivot.position);
    }

    private bool HasValidTarget()
    {
        return target != null && target.gameObject.activeInHierarchy;
    }
}
