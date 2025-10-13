using UnityEngine;

[CreateAssetMenu(fileName = "AbilityDash", menuName = "Scriptable Objects/AbilityDash")]
public class AbilityDash : BaseAbility
{
    private void OnValidate() { if (duration <= 0f) duration = 0.5f; }

    public override void OnActivate(in AbilityContext ctx, AbilityInstance inst)
    {
        ctx.Player.AirplaneDash();
        ctx.Ability.PlayerIsDashing(true);
    }

    public override void OnDeactivate(in AbilityContext ctx, AbilityInstance inst, bool cancelled)
    {
        ctx.Player.AirplaneDashEnd();
        ctx.Ability.PlayerIsDashing(false);
    }
}
