using UnityEngine;

public class ExtractionObjective : Objective, IInteractable
{
    private enum Phase { Waiting, Holdout, Boarding }

    [Header("Holdout")]
    [SerializeField] private float holdoutDuration = 90f;
    [SerializeField] private WaveSpawner waveSpawner;
    [SerializeField] private WaveDefinition wave;

    [Header("Boarding")]
    [SerializeField] private ExtractionShip ship;
    [SerializeField] private Transform boardingCenter;
    [SerializeField] private float boardingRadius = 4f;
    [SerializeField] private LayerMask playerMask;

    private readonly Collider[] boardingBuffer = new Collider[1];

    private Phase phase;
    private float remainingTime;
    private int shownSeconds;
    private bool wasShipLanded;

    public LocalizedMessage InteractionPrompt => State == ObjectiveState.Active && phase == Phase.Waiting ? new LocalizedMessage("[F] Call extraction") : LocalizedMessage.Empty;

    protected override void OnActivated()
    {
        GameEvents.OnNotification("Extraction available");
    }

    public void Interact(GameObject interactor)
    {
        if (State != ObjectiveState.Active || phase != Phase.Waiting) return;

        phase = Phase.Holdout;
        remainingTime = holdoutDuration;
        shownSeconds = Mathf.CeilToInt(remainingTime);

        waveSpawner.Begin(wave, holdoutDuration);
        GameEvents.OnMusicIntensityChanged(1f);

        GameEvents.OnNotification("Extraction inbound. Hold the area!");
        NotifyChanged();
    }

    private void Update()
    {
        if (State != ObjectiveState.Active) return;

        if (phase == Phase.Holdout)
            UpdateHoldout();
        else if (phase == Phase.Boarding)
            UpdateBoarding();
    }

    private void UpdateHoldout()
    {
        remainingTime -= Time.deltaTime;

        int seconds = Mathf.CeilToInt(Mathf.Max(0f, remainingTime));

        if (seconds != shownSeconds)
        {
            shownSeconds = seconds;
            NotifyChanged();
        }

        if (remainingTime > 0f) return;

        waveSpawner.Stop();
        ship.Land();
        GameEvents.OnMusicIntensityChanged(0.6f);
        phase = Phase.Boarding;

        GameEvents.OnNotification("The ship is landing!");
        NotifyChanged();
    }

    private void UpdateBoarding()
    {
        if (ship.IsLanded != wasShipLanded)
        {
            wasShipLanded = ship.IsLanded;
            NotifyChanged();
        }

        if (ship.IsLanded && IsPlayerBoarding())
            Complete();
    }

    private bool IsPlayerBoarding()
    {
        return Physics.OverlapSphereNonAlloc(boardingCenter.position, boardingRadius, boardingBuffer, playerMask, QueryTriggerInteraction.Ignore) > 0;
    }

    protected override LocalizedMessage GetProgress()
    {
        switch (phase)
        {
            case Phase.Holdout:
                int total = Mathf.CeilToInt(Mathf.Max(0f, remainingTime));
                return new LocalizedMessage("Hold out {0}", $"{total / 60}:{total % 60:00}");
            case Phase.Boarding:
                return new LocalizedMessage(ship.IsLanded ? "Board the ship" : "Ship landing...");
            default:
                return new LocalizedMessage("Call the extraction");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (boardingCenter == null) return;

        Gizmos.color = new Color(0.3f, 1f, 0.5f, 0.5f);
        Gizmos.DrawWireSphere(boardingCenter.position, boardingRadius);
    }
}
