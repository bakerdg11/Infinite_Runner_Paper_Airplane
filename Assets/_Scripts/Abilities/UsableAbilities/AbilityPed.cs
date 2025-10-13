using UnityEngine;

[CreateAssetMenu(fileName = "AbilityPed", menuName = "Scriptable Objects/AbilityPed")]
public class AbilityPed : BaseAbility
{
    private void OnValidate() { if (duration <= 0f) duration = 5f; }

    public override void OnActivate(in AbilityContext ctx, AbilityInstance inst)
    {
        if (ctx.Upgrades != null) ctx.Upgrades.energyDepletionPaused = true;
    }

    public override void OnDeactivate(in AbilityContext ctx, AbilityInstance inst, bool cancelled)
    {
        if (ctx.Upgrades != null) ctx.Upgrades.energyDepletionPaused = false;
    }
}
