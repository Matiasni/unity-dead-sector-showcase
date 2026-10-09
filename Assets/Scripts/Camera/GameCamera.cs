using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GameCamera : MonoBehaviour
{
    public static Camera Current { get; private set; }

    private Camera ownCamera;

    private void Awake()
    {
        ownCamera = GetComponent<Camera>();
        Current = ownCamera;
    }

    private void OnDestroy()
    {
        if (Current == ownCamera)
            Current = null;
    }
}
