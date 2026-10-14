
using UnityEngine;

public class SharkAttack : MonoBehaviour
{
    private bool hasAttacked = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryAttack(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryAttack(collision.gameObject);
    }

    private void TryAttack(GameObject other)
    {
        if (hasAttacked)
            return;

        if (!other.CompareTag("Player"))
            return;

        OxygenSystem oxygenSystem =
            FindFirstObjectByType<OxygenSystem>();

        if (oxygenSystem != null)
        {
            hasAttacked = true;
            oxygenSystem.SharkAttack();
        }
        else
        {
            Debug.LogError("OxygenSystem was not found!");
        }
    }
}
