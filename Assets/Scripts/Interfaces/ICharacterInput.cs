using UnityEngine;
using System;

public interface ICharacterInput
{
    Vector2 Move { get; }
    bool ShootHeld { get; }
    bool AimHeld { get; }
}

public interface ICharacterInputEvents
{
    event Action OnShootStarted;
    event Action OnShootCanceled;

    event Action OnAimStarted;
    event Action OnAimCanceled;
}
