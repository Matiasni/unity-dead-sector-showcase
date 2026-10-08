using UnityEngine;

public class PatrolRoute : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;

    public int Count => waypoints.Length;

    public Vector3 GetPoint(int index) => waypoints[index % waypoints.Length].position;

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        Gizmos.color = new Color(1f, 0.6f, 0.1f, 0.8f);

        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] != null && waypoints[(i + 1) % waypoints.Length] != null)
                Gizmos.DrawLine(waypoints[i].position, waypoints[(i + 1) % waypoints.Length].position);
        }
    }
}
