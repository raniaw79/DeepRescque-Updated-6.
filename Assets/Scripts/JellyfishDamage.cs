using UnityEngine;

public class JellyfishDamage : MonoBehaviour
{
    public float damageAmount = 10f;
    public float damageCooldown = 1.5f;

    private float nextDamageTime = 0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (Time.time < nextDamageTime)
            return;

        nextDamageTime = Time.time + damageCooldown;

        // البحث الآمن عن نظام الأكسجين في المشهد
        OxygenSystem oxygenSystem = FindFirstObjectByType<OxygenSystem>();

        if (oxygenSystem != null)
        {
            oxygenSystem.DamageOxygen(damageAmount);
        }
        else
        {
            Debug.LogError("OxygenSystem not found in the scene!");
        }

        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.TakeHit(transform.position);
        }
    }
}