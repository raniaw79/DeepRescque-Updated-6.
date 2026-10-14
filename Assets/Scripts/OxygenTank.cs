using UnityEngine;

public class OxygenTank : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OxygenSystem oxygenSystem = FindFirstObjectByType<OxygenSystem>();

            if (oxygenSystem != null && oxygenSystem.oxygenTanks < oxygenSystem.maxOxygenTanks)
            {
                oxygenSystem.AddOxygenTank();
                Destroy(gameObject);
            }
        }
    }
}