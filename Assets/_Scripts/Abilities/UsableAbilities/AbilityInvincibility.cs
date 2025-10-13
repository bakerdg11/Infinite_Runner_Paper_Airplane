using UnityEngine;

[CreateAssetMenu(fileName = "AbilityInvincibility", menuName = "Scriptable Objects/AbilityInvincibility")]
public class AbilityInvincibility : BaseAbility
{
    private void OnValidate() { if (duration <= 0f) duration = 3f; } // choose your default

    public override void OnActivate(in AbilityContext ctx, AbilityInstance inst)
    {
        if (ctx.Upgrades != null) ctx.Upgrades.invincibleEnabled = true;
    }

    public override void OnDeactivate(in AbilityContext ctx, AbilityInstance inst, bool cancelled)
    {
        if (ctx.Upgrades != null) ctx.Upgrades.invincibleEnabled = false;
    }
}
