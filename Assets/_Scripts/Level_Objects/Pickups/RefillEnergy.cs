using UnityEngine;

public class RefillEnergy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // make sure the thing that touched us is the player
        if (!other.CompareTag("Player")) return;

        var pc = PlayerManager.Instance?.PlayerController;
        pc?.RefillEnergy();
        Destroy(gameObject);
    }
}