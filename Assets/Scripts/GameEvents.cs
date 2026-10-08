using System;

public static class GameEvents
{
    public static Action<int, int> onPlayerHealthChanged;
    public static Action<WeaponSettings[], int> onLoadoutChanged;
    public static Action<LocalizedMessage> onInteractionPromptChanged;
    public static Action<AbilityLoadout> onAbilitiesChanged;
    public static Action<ResourceDefinition, int, int> onPlayerResourceChanged;
    public static Action<LocalizedMessage> onNotification;
    public static Action<int, WeaponStatus> onWeaponStatusChanged;
    public static Action<ObjectiveStatus[]> onObjectivesChanged;
    public static Action<int> onReinforcementsChanged;
    public static Action<MissionResult> onMissionEnded;
    public static Action<ExperienceStatus> onExperienceChanged;
    public static Action<int> onPlayerLevelUp;
    public static Action<UpgradeDefinition[]> onUpgradesOffered;
    public static Action<UpgradeDefinition> onUpgradeSelected;
    public static Action<float> onCameraShake;
    public static Action<string, float> onTutorialHint;
    public static Action<float> onMusicIntensityChanged;
    public static Action<int> onEnemyKilled;

    public static Action<float> onUpdate;

    public static void OnPlayerHealthChanged(int currentHealth, int maxHealth)
    {
        onPlayerHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public static void OnLoadoutChanged(WeaponSettings[] loadout, int activeIndex)
    {
        onLoadoutChanged?.Invoke(loadout, activeIndex);
    }

    public static void OnInteractionPromptChanged(LocalizedMessage prompt)
    {
        onInteractionPromptChanged?.Invoke(prompt);
    }

    public static void OnAbilitiesChanged(AbilityLoadout loadout)
    {
        onAbilitiesChanged?.Invoke(loadout);
    }

    public static void OnPlayerResourceChanged(ResourceDefinition resource, int amount, int delta)
    {
        onPlayerResourceChanged?.Invoke(resource, amount, delta);
    }

    public static void OnNotification(string key, params object[] args)
    {
        onNotification?.Invoke(new LocalizedMessage(key, args));
    }

    public static void OnWeaponStatusChanged(int slot, WeaponStatus status)
    {
        onWeaponStatusChanged?.Invoke(slot, status);
    }

    public static void OnObjectivesChanged(ObjectiveStatus[] objectives)
    {
        onObjectivesChanged?.Invoke(objectives);
    }

    public static void OnReinforcementsChanged(int remaining)
    {
        onReinforcementsChanged?.Invoke(remaining);
    }

    public static void OnMissionEnded(MissionResult result)
    {
        onMissionEnded?.Invoke(result);
    }

    public static void OnExperienceChanged(ExperienceStatus status)
    {
        onExperienceChanged?.Invoke(status);
    }

    public static void OnPlayerLevelUp(int level)
    {
        onPlayerLevelUp?.Invoke(level);
    }

    public static void OnUpgradesOffered(UpgradeDefinition[] options)
    {
        onUpgradesOffered?.Invoke(options);
    }

    public static void OnUpgradeSelected(UpgradeDefinition upgrade)
    {
        onUpgradeSelected?.Invoke(upgrade);
    }

    public static void OnCameraShake(float trauma)
    {
        onCameraShake?.Invoke(trauma);
    }

    public static void OnTutorialHint(string hint, float duration)
    {
        onTutorialHint?.Invoke(hint, duration);
    }

    public static void OnMusicIntensityChanged(float intensity)
    {
        onMusicIntensityChanged?.Invoke(intensity);
    }

    public static void OnEnemyKilled(int experience)
    {
        onEnemyKilled?.Invoke(experience);
    }

    public static void OnUdate(float timeDelta)
    {
        onUpdate?.Invoke(timeDelta);
    }

}
