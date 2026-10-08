using System;
using System.Linq;
using UnityEngine;

public class MissionDirector : MonoBehaviour
{
    [SerializeField] private MissionDefinition definition;

    [Header("Objectives")]
    [SerializeField] private Objective[] requiredObjectives;
    [SerializeField] private Objective[] optionalObjectives;
    [SerializeField] private Objective finalObjective;

    [Header("Player")]
    [SerializeField] private ResourceWallet playerWallet;
    [SerializeField] private ReinforcementSystem reinforcements;

    private readonly StateMachine<IState> stateMachine = new();

    private Objective[] allObjectives;
    private IState objectivesPhase;
    private bool isOutOfReinforcements;
    private bool hasEnded;

    public event Action<MissionResult> OnMissionEnded;

    private void Awake()
    {
        allObjectives = requiredObjectives.Concat(optionalObjectives).Append(finalObjective).ToArray();
        BuildStateMachine();
    }

    private void BuildStateMachine()
    {
        objectivesPhase = new ActivateObjectivesState(playerWallet, requiredObjectives.Concat(optionalObjectives).ToArray());
        var finalPhase = new ActivateObjectivesState(playerWallet, finalObjective);
        var success = new MissionEndState(() => EndMission(true));
        var failure = new MissionEndState(() => EndMission(false));

        stateMachine.AddTransition(objectivesPhase, finalPhase, () => requiredObjectives.All(objective => objective.IsCompleted));
        stateMachine.AddTransition(finalPhase, success, () => finalObjective.IsCompleted);
        stateMachine.AddAnyTransition(failure, () => isOutOfReinforcements && !hasEnded);
    }

    private void OnEnable()
    {
        foreach (var objective in allObjectives)
            objective.OnChanged += BroadcastObjectives;

        reinforcements.OnReinforcementsChanged += GameEvents.OnReinforcementsChanged;
        reinforcements.OnDepleted += HandleReinforcementsDepleted;
    }

    private void OnDisable()
    {
        foreach (var objective in allObjectives)
            objective.OnChanged -= BroadcastObjectives;

        reinforcements.OnReinforcementsChanged -= GameEvents.OnReinforcementsChanged;
        reinforcements.OnDepleted -= HandleReinforcementsDepleted;
    }

    private void Start()
    {
        GamePause.Clear();

        reinforcements.Setup(definition.reinforcements);
        stateMachine.SetState(objectivesPhase);

        BroadcastObjectives(null);
    }

    private void Update()
    {
        stateMachine.Tick(Time.deltaTime);
    }

    private void HandleReinforcementsDepleted()
    {
        isOutOfReinforcements = true;
    }

    private void BroadcastObjectives(Objective changed)
    {
        GameEvents.OnObjectivesChanged(allObjectives.Select(objective => objective.GetStatus()).ToArray());
    }

    private void EndMission(bool success)
    {
        if (hasEnded) return;

        hasEnded = true;

        var result = MissionResult.From(definition, playerWallet, success);

        GamePause.Request(this);

        OnMissionEnded?.Invoke(result);
        GameEvents.OnMissionEnded(result);
    }
}
