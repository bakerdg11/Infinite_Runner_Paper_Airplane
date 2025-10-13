using UnityEngine;

[CreateAssetMenu(fileName = "AbilityBoost", menuName = "Scriptable Objects/AbilityBoost")]
public class AbilityBoost : BaseAbility
{
    private void OnValidate() { if (duration <= 0f) duration = 4f; }

    public override void OnActivate(in AbilityContext ctx, AbilityInstance inst)
    {
        ctx.Player.AirplaneBoost();
    }

    public override void OnDeactivate(in AbilityContext ctx, AbilityInstance inst, bool cancelled)
    {
        ctx.Player.AirplaneBoostEnd();
    }
}
