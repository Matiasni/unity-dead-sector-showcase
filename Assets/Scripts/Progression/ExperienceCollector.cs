using UnityEngine;

public class ExperienceCollector : MonoBehaviour
{
    [SerializeField] private ResourceDefinition experienceResource;

    private IResourceWallet wallet;

    private void Awake()
    {
        wallet = GetComponent<IResourceWallet>();
    }

    private void OnEnable()
    {
        GameEvents.onEnemyKilled += CollectExperience;
    }

    private void OnDisable()
    {
        GameEvents.onEnemyKilled -= CollectExperience;
    }

    private void CollectExperience(int experience)
    {
        wallet?.Add(experienceResource, experience);
    }
}
