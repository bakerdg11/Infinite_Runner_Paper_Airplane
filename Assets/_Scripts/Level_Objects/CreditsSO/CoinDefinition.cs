using UnityEngine;

[CreateAssetMenu(fileName = "CoinDefinition", menuName = "Scriptable Objects/CoinDefinition")]
public class CoinDefinition : ScriptableObject
{
    public string coinId;
    public int creditValue;

    [Header("Visuals")]
    public GameObject coinPrefab;
    public Mesh mesh;
    public Material material;

    [Header("Spawn")]
    [Range(0f, 1f)] public float spawnWeight; // for weighted random spawn. Higher the number the higher the chance. 0.7, 0.25, 0.05

    [Header("FX (optional)")]
    public AudioClip pickupSfx;
    public GameObject pickupVfxPrefab;
}
