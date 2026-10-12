using UnityEngine;

public class PickupGlow : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] systems;

    private float[] baseAlphas;

    private void Awake()
    {
        baseAlphas = new float[systems.Length];

        for (int i = 0; i < systems.Length; i++)
            baseAlphas[i] = systems[i].main.startColor.color.a;
    }

    public void SetColor(Color color)
    {
        for (int i = 0; i < systems.Length; i++)
        {
            var main = systems[i].main;
            main.startColor = new Color(color.r, color.g, color.b, baseAlphas[i]);

            systems[i].Clear(false);
            systems[i].Play(false);
        }
    }
}
