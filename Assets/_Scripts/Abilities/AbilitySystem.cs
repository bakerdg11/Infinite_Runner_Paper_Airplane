using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySystem : MonoBehaviour
{
    [SerializeField] private List<BaseAbility> loadout;

    private List<AbilityInstance> _instances;
    private PlayerController _player;
    private StatsManager _stats;
    private UpgradesManager _upgrades;
    private AbilitySystem _abilities;

    [Header("Ability Bools")]
    public bool isDashing {  get; private set; }
    public bool isInvincible { get; private set; }
    public bool energyDepletionPaused { get; private set; }

    // Optional event so HUD can subscribe; or we can call HUD directly
    public event Action<string, int> OnAmmoChanged;  // (abilityId, chargesLeft)

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
        _stats = StatsManager.Instance;
        _upgrades = UpgradesManager.Instance;

        _instances = new(loadout.Count);
        foreach (var def in loadout) _instances.Add(new AbilityInstance(def));
    }

    void Update()
    {
        float dt = Time.deltaTime;
        var ctx = BuildCtx();

        for (int i = 0; i < _instances.Count; i++)
        {
            var inst = _instances[i];
            var def = inst.def;
            if (!inst.active) continue;

            if (def.duration > 0f)
            {
                inst.timeLeft -= dt;
                def.OnTick(ctx, inst, dt);
                if (inst.timeLeft <= 0f)
                {
                    EndAbility(inst, cancelled: false);
                }
            }
        }
    }

    private AbilityContext BuildCtx() => new AbilityContext(_player, _stats, _upgrades, this);

    public bool TryActivate(string abilityId)
    {
        int idx = loadout.FindIndex(a => a.abilityId == abilityId);
        if (idx < 0) return false;

        var def = loadout[idx];
        var inst = _instances[idx];

        var ctx = BuildCtx();
        if (!def.CanActivate(ctx, inst)) return false;

        // consume ammo
        inst.chargesLeft = Mathf.Max(0, inst.chargesLeft - 1);
        NotifyAmmo(def.abilityId, inst.chargesLeft);

        // start/refresh duration or instant fire
        if (def.duration > 0f)
        {
            inst.active = true;
            inst.timeLeft = def.duration;
            def.OnActivate(ctx, inst);
        }
        else
        {
            // instant effect (e.g., missile)
            def.OnActivate(ctx, inst);
            def.OnDeactivate(ctx, inst, cancelled: false); // nothing to keep; consistent lifecycle
        }

        return true;
    }



    public void Cancel(string abilityId)
    {
        var inst = GetInstance(abilityId);
        if (inst == null || !inst.active) return;
        EndAbility(inst, cancelled: true);
    }

    private void EndAbility(AbilityInstance inst, bool cancelled)
    {
        inst.def.OnDeactivate(BuildCtx(), inst, cancelled);
        inst.active = false;
        inst.timeLeft = 0f;
    }

    public void BeginNewRun()  // call when respawning/reloading level
    {
        foreach (var inst in _instances)
        {
            inst.Reset();
            NotifyAmmo(inst.def.abilityId, inst.chargesLeft);
        }
    }

    public AbilityInstance GetInstance(string id)
    {
        int idx = loadout.FindIndex(a => a.abilityId == id);
        return idx >= 0 ? _instances[idx] : null;
    }

    private void NotifyAmmo(string id, int count)
    {
        OnAmmoChanged?.Invoke(id, count);
        HUD.Instance?.SetAbilityAmmo(id, count); // direct update if you prefer
    }





    // ------------------------------------------------------------------- Ability Bools ------------------
    public void PlayerIsDashing(bool value)
    {
        isDashing = value;
    }

    public void PlayerIsInvincible(bool value)
    {
        isInvincible = value;
    }

    public void SetEnergyDepletionPaused(bool value)
    {
        energyDepletionPaused = value;
        // (Optional) tell HUD to show/hide a small indicator or bar
        // HUD.Instance?.ShowEnergyPause(value);
    }






}