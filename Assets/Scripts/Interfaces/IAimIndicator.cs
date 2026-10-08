using UnityEngine;

public interface IAimIndicator
{
    void Show(Vector3 from, Vector3 to, Color color);
    void Hide();
}
