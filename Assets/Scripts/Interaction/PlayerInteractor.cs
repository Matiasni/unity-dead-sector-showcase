using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    private IInteractionInput input;

    private readonly List<IInteractable> nearby = new();
    private IInteractable focused;

    public event Action<IInteractable> OnFocusChanged;

    private void Awake()
    {
        input = GetComponent<IInteractionInput>();
    }

    private void Update()
    {
        if (focused == null || input == null) return;

        if (input.InteractPressed)
            focused.Interact(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        var interactable = other.GetComponentInParent<IInteractable>();

        if (interactable == null || nearby.Contains(interactable)) return;

        nearby.Add(interactable);
        RefreshFocus();
    }

    private void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponentInParent<IInteractable>();

        if (interactable == null) return;

        nearby.Remove(interactable);
        RefreshFocus();
    }

    private void OnDisable()
    {
        nearby.Clear();
        RefreshFocus();
    }

    private void RefreshFocus()
    {
        var next = nearby.Count > 0 ? nearby[^1] : null;

        if (next == focused) return;

        focused = next;
        OnFocusChanged?.Invoke(focused);
    }
}
