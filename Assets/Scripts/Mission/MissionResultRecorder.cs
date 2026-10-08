using UnityEngine;

[RequireComponent(typeof(MissionDirector))]
public class MissionResultRecorder : MonoBehaviour
{
    private MissionDirector director;

    private void Awake()
    {
        director = GetComponent<MissionDirector>();
    }

    private void OnEnable()
    {
        director.OnMissionEnded += GameSession.RecordResult;
    }

    private void OnDisable()
    {
        director.OnMissionEnded -= GameSession.RecordResult;
    }
}
