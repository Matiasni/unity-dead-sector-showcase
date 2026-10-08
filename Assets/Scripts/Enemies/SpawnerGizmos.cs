using UnityEngine;

public static class SpawnerGizmos
{
    public static readonly Color WaveColor = new(1f, 0.55f, 0.1f);
    public static readonly Color NestColor = new(0.85f, 0.2f, 0.6f);
    public static readonly Color StationColor = new(0.3f, 0.75f, 1f);
    public static readonly Color EncounterColor = new(1f, 0.85f, 0.2f);
    public static readonly Color DummyColor = new(0.6f, 1f, 0.4f);

    private const float MarkerHeight = 2.5f;

    public static void DrawMarker(Vector3 position, Color color, string label)
    {
        Vector3 top = position + Vector3.up * MarkerHeight;

        Gizmos.color = color;
        Gizmos.DrawLine(position, top);
        Gizmos.DrawSphere(top, 0.35f);

        Gizmos.color = WithAlpha(color, 0.25f);
        Gizmos.DrawSphere(position, 0.5f);

        DrawLabel(top + Vector3.up * 0.6f, color, label);
    }

    public static void DrawSpawnPoint(Vector3 origin, Vector3 point, Color color, float radius, string label = null)
    {
        Gizmos.color = WithAlpha(color, 0.5f);
        Gizmos.DrawLine(origin, point);

        Gizmos.color = color;
        Gizmos.DrawWireSphere(point, Mathf.Max(0.4f, radius));

        Gizmos.color = WithAlpha(color, 0.35f);
        Gizmos.DrawSphere(point, 0.3f);

        if (!string.IsNullOrEmpty(label))
            DrawLabel(point + Vector3.up * 1.2f, color, label);
    }

    public static void DrawArea(Transform transform, BoxCollider area, Color color)
    {
        if (area == null) return;

        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = WithAlpha(color, 0.08f);
        Gizmos.DrawCube(area.center, area.size);

        Gizmos.color = WithAlpha(color, 0.8f);
        Gizmos.DrawWireCube(area.center, area.size);

        Gizmos.matrix = Matrix4x4.identity;
    }

    public static void DrawLabel(Vector3 position, Color color, string text)
    {
#if UNITY_EDITOR
        var style = new GUIStyle(UnityEditor.EditorStyles.boldLabel) { normal = { textColor = color } };
        UnityEditor.Handles.Label(position, text, style);
#endif
    }

    private static Color WithAlpha(Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }
}
