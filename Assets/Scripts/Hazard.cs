using UnityEngine;

public class Hazard : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OxygenSystem oxygenSystem = FindFirstObjectByType<OxygenSystem>();

            if (oxygenSystem != null)
            {
                oxygenSystem.DamageOxygen(40f);
            }
        }
    }
}