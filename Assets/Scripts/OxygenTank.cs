
using UnityEngine;

public class OxygenTank : MonoBehaviour
{
    [Header("Floating Movement")]
    public float floatHeight = 0.15f;
    public float floatSpeed = 1.5f;
    public float rotationAmount = 5f;
    public float rotationSpeed = 1f;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private float randomOffset;

    private void OnEnable()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        randomOffset = Random.Range(0f, 10f);
    }

    private void Update()
    {
        float verticalMovement =
            Mathf.Sin((Time.time + randomOffset) * floatSpeed)
            * floatHeight;

        transform.position = startPosition
            + Vector3.up * verticalMovement;

        float rotation =
            Mathf.Sin((Time.time + randomOffset) * rotationSpeed)
            * rotationAmount;

        transform.rotation = startRotation
            * Quaternion.Euler(0f, 0f, rotation);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OxygenSystem oxygenSystem =
                FindFirstObjectByType<OxygenSystem>();

            if (oxygenSystem != null &&
                oxygenSystem.oxygenTanks < oxygenSystem.maxOxygenTanks)
            {
                oxygenSystem.AddOxygenTank();
                Destroy(gameObject);
            }
        }
    }
}
