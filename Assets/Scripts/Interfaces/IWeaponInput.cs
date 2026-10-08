public interface IWeaponInput
{
    bool SwitchWeaponPressed { get; }
    bool ReloadPressed { get; }
    int SelectedSlot { get; }
}
