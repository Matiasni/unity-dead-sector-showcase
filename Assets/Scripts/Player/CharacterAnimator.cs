using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private MovementBehaviour movement;
    private CharacterCombatState combat;

    private void Awake()
    {
        movement = GetComponent<MovementBehaviour>();
        combat = GetComponent<CharacterCombatState>();
    }

    private void Update()
    {
        UpdateMovement();
        UpdateCombat();
    }

    private void UpdateMovement()
    {
        Vector3 localMove = transform.InverseTransformDirection(movement.MoveDirection);
        animator.SetFloat("X", localMove.x);
        animator.SetFloat("Y", localMove.z);
    }

    private void UpdateCombat()
    {
        animator.SetBool("IsAiming", combat.IsAiming);
    }
}