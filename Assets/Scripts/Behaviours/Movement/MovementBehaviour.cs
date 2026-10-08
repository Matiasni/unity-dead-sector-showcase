using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementBehaviour : MonoBehaviour, IDashMotor, IKnockbackReceiver
{
    [SerializeField] private PlayerConfig config;

    private Rigidbody rb;

    private Vector3 moveDirection;
    public Vector3 MoveDirection => moveDirection;

    private Vector3 forcedVelocity;
    private float forcedVelocityEndTime;

    public bool IsDashing => Time.time < forcedVelocityEndTime;

    private readonly List<IMovementModifier> modifiers = new();

    public event Action OnDashPerformed;

    public PlayerConfig Config => config;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetMoveInput(Vector2 input)
    {
        moveDirection = new Vector3(input.x, 0, input.y);
    }

    public void Dash(Vector3 direction, float speed, float duration)
    {
        direction.y = 0f;

        ForceVelocity(direction.normalized * speed, duration);
        OnDashPerformed?.Invoke();
    }

    public void ApplyKnockback(Vector3 velocity, float duration)
    {
        velocity.y = 0f;

        ForceVelocity(velocity, duration);
    }

    private void ForceVelocity(Vector3 velocity, float duration)
    {
        forcedVelocity = velocity;
        forcedVelocityEndTime = Time.time + duration;
    }

    private void Update()
    {
        UpdateModifiers();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 velocity = IsDashing
            ? forcedVelocity
            : moveDirection * config.Movement.moveSpeed * GetFinalSpeedMultiplier();

        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;
    }

    private float GetFinalSpeedMultiplier()
    {
        float result = 1f;

        foreach (var mod in modifiers)
            result *= mod.GetSpeedMultiplier();

        return result;
    }

    private void UpdateModifiers()
    {
        float dt = Time.deltaTime;

        for (int i = modifiers.Count - 1; i >= 0; i--)
        {
            var mod = modifiers[i];

            mod.Tick(dt);

            if (mod.IsFinished)
                modifiers.RemoveAt(i);
        }
    }

    public void AddModifier(IMovementModifier modifier)
    {
        if (!modifiers.Contains(modifier))
            modifiers.Add(modifier);
    }

    public void RemoveModifier(IMovementModifier modifier)
    {
        modifiers.Remove(modifier);
    }
}
