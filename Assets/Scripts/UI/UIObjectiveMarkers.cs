using UnityEngine;
using UnityEngine.UI;

public class UIObjectiveMarkers : MonoBehaviour
{
    [SerializeField] private RectTransform[] markers;
    [SerializeField] private Camera worldCamera;
    [SerializeField] private float heightOffset = 3f;
    [SerializeField] private float screenPadding = 40f;

    private ObjectiveStatus[] objectives = new ObjectiveStatus[0];
    private Text[] labels;

    private void Awake()
    {
        if (worldCamera == null)
            worldCamera = Camera.main;

        labels = new Text[markers.Length];

        for (int i = 0; i < markers.Length; i++)
        {
            labels[i] = markers[i].GetComponentInChildren<Text>();
            markers[i].gameObject.SetActive(false);
        }
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
        int markerIndex = 0;

        foreach (var objective in objectives)
        {
            if (objective.State != ObjectiveState.Active || markerIndex >= markers.Length) continue;

            PlaceMarker(markers[markerIndex], labels[markerIndex], objective);
            markerIndex++;
        }

        for (int i = markerIndex; i < markers.Length; i++)
            markers[i].gameObject.SetActive(false);
    }

    private void PlaceMarker(RectTransform marker, Text label, ObjectiveStatus objective)
    {
        Vector3 screen = worldCamera.WorldToScreenPoint(objective.Position + Vector3.up * heightOffset);

        if (screen.z < 0f)
            screen = new Vector3(Screen.width - screen.x, Screen.height - screen.y, 0f);

        screen.x = Mathf.Clamp(screen.x, screenPadding, Screen.width - screenPadding);
        screen.y = Mathf.Clamp(screen.y, screenPadding, Screen.height - screenPadding);

        marker.gameObject.SetActive(true);
        marker.position = new Vector3(screen.x, screen.y, 0f);

        if (label != null)
            label.text = objective.Label;
    }
}
