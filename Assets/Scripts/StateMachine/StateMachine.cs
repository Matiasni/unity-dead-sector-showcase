using System;
using System.Collections.Generic;

public class StateMachine<TState> where TState : class, IState
{
    private readonly struct Transition
    {
        public readonly TState To;
        public readonly Func<bool> Condition;

        public Transition(TState to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }

    private static readonly List<Transition> NoTransitions = new(0);

    private readonly Dictionary<TState, List<Transition>> transitions = new();
    private readonly List<Transition> anyTransitions = new();

    private List<Transition> currentTransitions = NoTransitions;

    public TState CurrentState { get; private set; }

    public event Action<TState, TState> OnStateChanged;

    public void AddTransition(TState from, TState to, Func<bool> condition)
    {
        if (!transitions.TryGetValue(from, out var list))
        {
            list = new List<Transition>();
            transitions.Add(from, list);
        }

        list.Add(new Transition(to, condition));
    }

    public void AddAnyTransition(TState to, Func<bool> condition)
    {
        anyTransitions.Add(new Transition(to, condition));
    }

    public void SetState(TState state)
    {
        if (state == CurrentState) return;

        TState previous = CurrentState;
        previous?.Exit();

        CurrentState = state;
        currentTransitions = state != null && transitions.TryGetValue(state, out var list) ? list : NoTransitions;

        CurrentState?.Enter();
        OnStateChanged?.Invoke(previous, CurrentState);
    }

    public void Tick(float deltaTime)
    {
        TState next = GetNextState();

        if (next != null)
            SetState(next);

        CurrentState?.Tick(deltaTime);
    }

    private TState GetNextState()
    {
        foreach (var transition in anyTransitions)
        {
            if (transition.To != CurrentState && transition.Condition())
                return transition.To;
        }

        foreach (var transition in currentTransitions)
        {
            if (transition.Condition())
                return transition.To;
        }

        return null;
    }
}
