
using UnityEngine;

public class TurtleRescue : MonoBehaviour
{
    public int scorePoints = 10;
    public GameObject rescueEffect;

    private bool rescued = false;

    private void Start()
    {
        Debug.Log("Turtle rescue ready: " + gameObject.name);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Turtle touched by: " + other.name);

        if (rescued || !other.CompareTag("Player"))
            return;

        rescued = true;

        if (rescueEffect != null)
        {
            GameObject effect = Instantiate(
                rescueEffect,
                transform.position,
                Quaternion.identity
            );

            effect.SetActive(true);
            Debug.Log("Rescue effect spawned: " + effect.name);
        }
        else
        {
            Debug.LogWarning("Rescue Effect is NOT assigned!");
        }

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scorePoints);
        }

        Destroy(gameObject);
    }
}
