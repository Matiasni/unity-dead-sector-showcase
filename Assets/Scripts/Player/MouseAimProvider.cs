using UnityEngine;
using UnityEngine.InputSystem;

public class MouseAimProvider : MonoBehaviour, IAimProvider
{
    [SerializeField] private Camera mainCamera;

    public Vector3 GetAimPoint()
    {
        if (mainCamera == null)
            mainCamera = GameCamera.Current;

        if (Mouse.current == null || mainCamera == null)
            return transform.position + transform.forward;

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane aimPlane = new Plane(Vector3.up, transform.position);

        if (aimPlane.Raycast(ray, out float distance))
            return ray.GetPoint(distance);

        return transform.position + transform.forward;
    }
}
