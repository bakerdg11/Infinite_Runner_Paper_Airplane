using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(Collider))]
public class Coin : MonoBehaviour
{
    [Header("Definitions")]
    [SerializeField] private CoinDefinition[] options; // assign Copper/Silver/Gold
    [SerializeField] private CoinDefinition overrideDefinition; // optional: force type

    [Header("Runtime")]
    [SerializeField] private CoinDefinition activeDef;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;

    [Header("Pickup")]
    public bool destroyOnPickup = true;

    void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();

        // Pick type
        if (overrideDefinition != null)
            activeDef = overrideDefinition;
        else
            activeDef = PickWeighted(options);

        ApplyVisuals(activeDef);
    }

    private void ApplyVisuals(CoinDefinition def)
    {
        if (def == null) return;
        if (def.mesh) _meshFilter.sharedMesh = def.mesh;
        if (def.material) _meshRenderer.sharedMaterial = def.material;
    }

    private static CoinDefinition PickWeighted(CoinDefinition[] defs)
    {
        if (defs == null || defs.Length == 0) return null;
        float total = 0f;
        foreach (var d in defs) total += Mathf.Max(0f, d ? d.spawnWeight : 0f);
        if (total <= 0f) return defs[0];

        float r = Random.value * total;
        foreach (var d in defs)
        {
            float w = Mathf.Max(0f, d ? d.spawnWeight : 0f);
            if (r < w) return d;
            r -= w;
        }
        return defs[defs.Length - 1];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Grant credits
        int amount = (activeDef != null) ? activeDef.creditValue : 1;
        StatsManager.Instance?.UpdatePickupCredits(amount);

        // Optional FX
        if (activeDef != null)
        {
            if (activeDef.pickupSfx)
                AudioSource.PlayClipAtPoint(activeDef.pickupSfx, transform.position);
            if (activeDef.pickupVfxPrefab)
                Instantiate(activeDef.pickupVfxPrefab, transform.position, Quaternion.identity);
        }

        if (destroyOnPickup)
            Destroy(gameObject);
        else
            gameObject.SetActive(false); // for pooling
    }
}