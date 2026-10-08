using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EnemyAimLaser : MonoBehaviour, IAimIndicator
{
    private LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.useWorldSpace = true;
        line.enabled = false;
    }

    private void OnDisable()
    {
        Hide();
    }

    public void Show(Vector3 from, Vector3 to, Color color)
    {
        line.startColor = color;
        line.endColor = color;
        line.SetPosition(0, from);
        line.SetPosition(1, to);
        line.enabled = true;
    }

    public void Hide()
    {
        if (line != null)
            line.enabled = false;
    }
}
