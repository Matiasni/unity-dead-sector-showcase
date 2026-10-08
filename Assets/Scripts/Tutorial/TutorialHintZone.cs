using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TutorialHintZone : MonoBehaviour
{
    [SerializeField, TextArea] private string hint;
    [SerializeField] private float duration = 6f;
    [SerializeField] private LayerMask playerMask;

    private bool hasShown;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasShown || (playerMask.value & (1 << other.gameObject.layer)) == 0) return;

        hasShown = true;
        GameEvents.OnTutorialHint(hint, duration);
    }
}
