using UnityEngine;

[CreateAssetMenu(fileName = "AbilityMissile", menuName = "Scriptable Objects/AbilityMissile")]
public class AbilityMissile : BaseAbility
{
    private void OnValidate()
    {
        duration = 0f;               // instant
        if (startingCharges <= 0) startingCharges = 3; // default ammo
    }

    public override void OnActivate(in AbilityContext ctx, AbilityInstance inst)
    {
        ctx.Player.FireMissile();
    }
}
