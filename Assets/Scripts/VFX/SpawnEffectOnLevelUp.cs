using UnityEngine;

[RequireComponent(typeof(LevelProgression))]
public class SpawnEffectOnLevelUp : MonoBehaviour
{
    [SerializeField] private PooledEffect effect;

    private LevelProgression progression;

    private void Awake()
    {
        progression = GetComponent<LevelProgression>();
    }

    private void OnEnable()
    {
        progression.OnLevelUp += Spawn;
    }

    private void OnDisable()
    {
        progression.OnLevelUp -= Spawn;
    }

    private void Spawn(int level)
    {
        PoolManager.Spawn(effect, transform);
    }
}
