using UnityEngine;

public class DashAfterimages : MonoBehaviour
{
    [SerializeField] private MeshFilter[] sources;
    [SerializeField] private Afterimage afterimagePrefab;
    [SerializeField] private float spawnInterval = 0.035f;

    private IDashMotor motor;
    private float nextSpawnTime;

    private void Awake()
    {
        motor = GetComponentInParent<IDashMotor>();
    }

    private void LateUpdate()
    {
        if (motor == null || !motor.IsDashing || Time.time < nextSpawnTime) return;

        nextSpawnTime = Time.time + spawnInterval;

        foreach (var source in sources)
        {
            if (!source.gameObject.activeInHierarchy) continue;

            var afterimage = PoolManager.Spawn(afterimagePrefab, source.transform.position, source.transform.rotation);
            afterimage.Show(source.sharedMesh, source.transform.lossyScale);
        }
    }
}
