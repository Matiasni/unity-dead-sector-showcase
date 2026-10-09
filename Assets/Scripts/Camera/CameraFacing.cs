using UnityEngine;

public class CameraFacing : MonoBehaviour
{
    private void LateUpdate()
    {
        var viewCamera = GameCamera.Current;

        if (viewCamera != null)
            transform.rotation = viewCamera.transform.rotation;
    }
}
