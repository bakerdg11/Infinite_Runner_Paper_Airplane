using UnityEngine;

public readonly struct AbilityContext
{
    public readonly PlayerController Player;
    public readonly StatsManager Stats;
    public readonly UpgradesManager Upgrades;
    public readonly AbilitySystem Ability;

    public AbilityContext(PlayerController p, StatsManager s, UpgradesManager u, AbilitySystem a)
    {
        Player = p;
        Stats = s;
        Upgrades = u;
        Ability = a;
    }
}

public sealed class AbilityInstance
{
    public readonly BaseAbility def;
    public bool active;
    public float timeLeft;
    public int chargesLeft;

    public AbilityInstance(BaseAbility def) { this.def = def; Reset(); }

    public void Reset()
    {
        active = false;
        timeLeft = 0f;
        chargesLeft = Mathf.Max(0, def.startingCharges);
    }
}