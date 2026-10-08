using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RotationBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerConfig playerConfig;

    private Rigidbody rb;

    private Vector3 lookDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        lookDirection = transform.forward;
    }

    public void SetLookDirection(Vector3 direction)
    {
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f) return;

        lookDirection = direction.normalized;
    }

    private void FixedUpdate()
    {
        Rotate();
    }

    private void Rotate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

        rb.MoveRotation(Quaternion.Slerp(
            rb.rotation,
            targetRotation,
            playerConfig.Rotation.rotationSpeed * Time.fixedDeltaTime
        ));
    }
}
