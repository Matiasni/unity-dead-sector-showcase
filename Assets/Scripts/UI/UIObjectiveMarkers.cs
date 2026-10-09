using UnityEngine;

public class UIObjectiveMarkers : MonoBehaviour
{
    [SerializeField] private UIObjectiveMarker[] markers;
    [SerializeField] private ObjectiveStyle style;
    [SerializeField] private Camera worldCamera;
    [SerializeField] private float heightOffset = 3f;
    [SerializeField] private float sidePadding = 60f;
    [SerializeField] private float topPadding = 250f;
    [SerializeField] private float bottomPadding = 190f;

    private ObjectiveStatus[] objectives = new ObjectiveStatus[0];
    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();

        foreach (var marker in markers)
            marker.Hide();
    }

    private void OnEnable()
    {
        GameEvents.onObjectivesChanged += UpdateObjectives;
    }

    private void OnDestroy()
    {
        GameEvents.onObjectivesChanged -= UpdateObjectives;
    }

    private void UpdateObjectives(ObjectiveStatus[] objectives)
    {
        this.objectives = objectives;
    }

    private void LateUpdate()
    {
        if (worldCamera == null)
            worldCamera = GameCamera.Current;

        if (worldCamera == null) return;

        int markerIndex = 0;

        foreach (var objective in objectives)
        {
            if (objective.State != ObjectiveState.Active || markerIndex >= markers.Length) continue;

            PlaceMarker(markers[markerIndex], objective);
            markerIndex++;
        }

        for (int i = markerIndex; i < markers.Length; i++)
            markers[i].Hide();
    }

    private void PlaceMarker(UIObjectiveMarker marker, ObjectiveStatus objective)
    {
        Vector3 screen = worldCamera.WorldToScreenPoint(objective.Position + Vector3.up * heightOffset);
        bool isBehind = screen.z < 0f;

        if (isBehind)
            screen = new Vector3(Screen.width - screen.x, Screen.height - screen.y, 0f);

        float scale = canvas.scaleFactor;
        var clamped = new Vector2(
            Mathf.Clamp(screen.x, sidePadding * scale, Screen.width - sidePadding * scale),
            Mathf.Clamp(screen.y, bottomPadding * scale, Screen.height - topPadding * scale));

        bool isOffscreen = isBehind || clamped.x != screen.x || clamped.y != screen.y;

        Vector2 fromCenter = (Vector2)screen - new Vector2(Screen.width, Screen.height) * 0.5f;
        float angle = Mathf.Atan2(fromCenter.y, fromCenter.x) * Mathf.Rad2Deg;

        marker.Show(style.GetMarker(objective), clamped, GetDistance(objective.Position), isOffscreen, angle);
    }

    private float GetDistance(Vector3 target)
    {
        Ray ray = worldCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        var ground = new Plane(Vector3.up, new Vector3(0f, target.y, 0f));

        Vector3 origin = ground.Raycast(ray, out float hit) ? ray.GetPoint(hit) : worldCamera.transform.position;
        origin.y = target.y;

        return Vector3.Distance(origin, target);
    }
}
