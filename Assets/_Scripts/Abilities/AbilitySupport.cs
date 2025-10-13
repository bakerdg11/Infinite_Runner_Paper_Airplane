using UnityEditor.PackageManager.Requests;
using UnityEngine;

public readonly struct AbilityContext
{
    public readonly PlayerController Player;
    public readonly StatsManager Stats;
    public readonly UpgradesManager Upgrades;

    public AbilityContext(PlayerController p, StatsManager s, UpgradesManager u)
    {
        Player = p;
        Stats = s;
        Upgrades = u;
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