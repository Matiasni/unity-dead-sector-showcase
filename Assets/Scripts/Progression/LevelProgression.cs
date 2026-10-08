using System;
using UnityEngine;

public class LevelProgression : MonoBehaviour
{
    [SerializeField] private LevelCurve curve;
    [SerializeField] private ResourceDefinition experienceResource;

    private IResourceWallet wallet;

    public int Level { get; private set; } = 1;

    public event Action<int> OnLevelUp;
    public event Action<ExperienceStatus> OnExperienceChanged;

    private void Awake()
    {
        wallet = GetComponent<IResourceWallet>();
    }

    private void OnEnable()
    {
        wallet.OnResourceChanged += HandleResourceChanged;
    }

    private void OnDisable()
    {
        wallet.OnResourceChanged -= HandleResourceChanged;
    }

    private void Start()
    {
        Level = curve.GetLevel(wallet.Get(experienceResource));
        NotifyExperience();
    }

    private void HandleResourceChanged(ResourceDefinition resource, int amount, int delta)
    {
        if (resource != experienceResource) return;

        int newLevel = curve.GetLevel(amount);

        while (Level < newLevel)
        {
            Level++;
            OnLevelUp?.Invoke(Level);
        }

        NotifyExperience();
    }

    private void NotifyExperience()
    {
        OnExperienceChanged?.Invoke(new ExperienceStatus(
            wallet.Get(experienceResource),
            Level,
            curve.GetThreshold(Level),
            curve.GetThreshold(Level + 1)
        ));
    }
}
