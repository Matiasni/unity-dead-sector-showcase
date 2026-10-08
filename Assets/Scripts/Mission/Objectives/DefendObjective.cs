using System.Collections;
using UnityEngine;

public class DefendObjective : Objective, IInteractable
{
    [Header("Defend")]
    [SerializeField] private HealthBehaviour defendTarget;
    [SerializeField] private float defendDuration = 60f;
    [SerializeField] private float resetDelay = 3f;
    [Tooltip("Layer the target uses while it can be attacked")]
    [SerializeField] private int targetableLayer = 11;

    [Header("Waves")]
    [SerializeField] private WaveSpawner waveSpawner;
    [SerializeField] private WaveDefinition wave;

    private int idleLayer;
    private float remainingTime;
    private int shownSeconds;
    private bool isDefending;
    private bool isResetting;

    public LocalizedMessage InteractionPrompt => CanStart ? new LocalizedMessage("[F] Start uplink") : LocalizedMessage.Empty;

    private bool CanStart => State == ObjectiveState.Active && !isDefending && !isResetting;

    private void Awake()
    {
        idleLayer = defendTarget.gameObject.layer;
    }

    public void Interact(GameObject interactor)
    {
        if (!CanStart) return;

        isDefending = true;
        remainingTime = defendDuration;
        shownSeconds = Mathf.CeilToInt(remainingTime);

        defendTarget.gameObject.layer = targetableLayer;
        defendTarget.OnDied += HandleTargetDestroyed;
        waveSpawner.Begin(wave, defendDuration);
        GameEvents.OnMusicIntensityChanged(0.7f);

        GameEvents.OnNotification("Uplink started. Defend the tower!");
        NotifyChanged();
    }

    private void Update()
    {
        if (!isDefending) return;

        remainingTime -= Time.deltaTime;

        int seconds = Mathf.CeilToInt(Mathf.Max(0f, remainingTime));

        if (seconds != shownSeconds)
        {
            shownSeconds = seconds;
            NotifyChanged();
        }

        if (remainingTime <= 0f)
        {
            EndDefense();
            Complete();
        }
    }

    private void HandleTargetDestroyed()
    {
        EndDefense();
        GameEvents.OnNotification("Uplink lost");
        StartCoroutine(ResetTarget());
    }

    private IEnumerator ResetTarget()
    {
        isResetting = true;
        NotifyChanged();

        yield return new WaitForSeconds(resetDelay);

        defendTarget.ResetHealth();
        isResetting = false;
        NotifyChanged();
    }

    private void EndDefense()
    {
        isDefending = false;

        defendTarget.OnDied -= HandleTargetDestroyed;
        defendTarget.gameObject.layer = idleLayer;
        waveSpawner.Stop();
        GameEvents.OnMusicIntensityChanged(0f);
    }

    protected override LocalizedMessage GetProgress()
    {
        if (isDefending)
            return new LocalizedMessage("Defend {0}  ({1}%)", FormatTime(remainingTime), Mathf.RoundToInt(100f * defendTarget.CurrentHealth / defendTarget.MaxHealth));

        if (isResetting)
            return new LocalizedMessage("Uplink lost, rebooting...");

        return new LocalizedMessage("Activate the tower");
    }

    private static string FormatTime(float seconds)
    {
        int total = Mathf.CeilToInt(Mathf.Max(0f, seconds));
        return $"{total / 60}:{total % 60:00}";
    }
}
