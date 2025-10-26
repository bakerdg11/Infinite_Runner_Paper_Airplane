using System;
using System.Collections;
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

    private void Start()
    {
        PushAllAmmoToHud();
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

                float normalized = Mathf.Clamp01(inst.timeLeft / def.duration);
                HUD.Instance?.SetAbilitySlider(def.abilityId, normalized, active: true);

                if (inst.timeLeft <= 0f)
                {
                    EndAbility(inst, cancelled: false);
                    HUD.Instance?.SetAbilitySlider(def.abilityId, 0f, active: false);
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

        if (inst.timeLeft > 0f && !inst.active)
        {
            Debug.Log($"[{def.abilityId}] still cooling down ({inst.timeLeft:F2}s left)");
            return false;
        }

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
            HUD.Instance?.SetAbilitySlider(def.abilityId, 1f, active: true);
        }
        else
        {
            // instant effect (e.g., missile)
            def.OnActivate(ctx, inst);
            def.OnDeactivate(ctx, inst, cancelled: false);

            // Start cooldown if ability defines one (e.g., missile)
            if (def.cooldown > 0f)
            {
                StartCoroutine(CooldownCountdown(inst, def));
            }
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

        if (inst.def.cooldown > 0f)
        {
            StartCoroutine(CooldownCountdown(inst, inst.def));
        }
    }

    private IEnumerator CooldownCountdown(AbilityInstance inst, BaseAbility def)
    {
        float remaining = def.cooldown;
        HUD.Instance?.SetAbilitySlider(def.abilityId, 1f, true);

        while (remaining > 0f)
        {
            remaining -= Time.deltaTime;
            float normalized = Mathf.Clamp01(remaining / def.cooldown);
            HUD.Instance?.SetAbilitySlider(def.abilityId, normalized, true);
            inst.timeLeft = remaining; // so TryActivate can check it
            yield return null;
        }

        HUD.Instance?.SetAbilitySlider(def.abilityId, 0f, false);
        inst.timeLeft = 0f;
    }

    public void BeginNewRun()  // call when respawning/reloading level
    {
        foreach (var inst in _instances)
        {
            inst.active = false;
            inst.timeLeft = 0f;

            // starting charges from the ScriptableObject (upgrades can add to this later)
            inst.chargesLeft = Mathf.Max(0, inst.def.startingCharges);
        }

        PushAllAmmoToHud(); // <-- show starting charges on the HUD
    }

    public void PushAllAmmoToHud()
    {
        if (HUD.Instance == null) return;
        foreach (var inst in _instances)
            HUD.Instance.SetAbilityAmmo(inst.def.abilityId, inst.chargesLeft);
    }

    public AbilityInstance GetInstance(string id)
    {
        int idx = loadout.FindIndex(a => a.abilityId == id);
        return idx >= 0 ? _instances[idx] : null;
    }

    private void NotifyAmmo(string id, int count)
    {
        HUD.Instance?.SetAbilityAmmo(id, count);
        OnAmmoChanged?.Invoke(id, count); // if you also use the event
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
    }






}