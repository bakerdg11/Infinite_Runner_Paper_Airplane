using UnityEngine;

[CreateAssetMenu(fileName = "BaseAbility", menuName = "Scriptable Objects/BaseAbility")]
public abstract class BaseAbility : ScriptableObject
{
    [Header("Meta")]
    public string abilityId;
    public Sprite icon;

    [Header("Timing & Ammo")]
    public float duration = 0f;
    public float cooldown = 0f;
    public int startingCharges = 0;
    public bool allowRefreshWhileActive = true;

    public virtual bool CanActivate(in AbilityContext ctx, AbilityInstance inst)
    {
        if (inst.chargesLeft <= 0) return false;
        if (!allowRefreshWhileActive && inst.active) return false;
        return true;
    }

    public abstract void OnActivate(in AbilityContext ctx, AbilityInstance inst);
    public virtual void OnTick(in AbilityContext ctx, AbilityInstance inst, float dt) { }
    public virtual void OnDeactivate(in AbilityContext ctx, AbilityInstance inst, bool cancelled) { }
}