
using UnityEngine;

public class TreasureCollect : MonoBehaviour
{
    [Header("Treasure Settings")]
    public int points = 10;

    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected)
            return;

        if (!other.CompareTag("Player"))
            return;

        if (ScoreManager.Instance == null)
        {
            Debug.LogError("ScoreManager not found!");
            return;
        }

        collected = true;

        ScoreManager.Instance.AddScore(points);

        Debug.Log("Treasure collected! +" + points);

        Destroy(gameObject);
    }
}
