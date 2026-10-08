using UnityEngine;

public class UIDoSomething : MonoBehaviour
{
    public void OnOpenScreen()
    {
        Debug.Log("OpenAnimation");
    }

    public void OnScreenClosed()
    {
        Debug.Log("Closed Animation");
    }
}