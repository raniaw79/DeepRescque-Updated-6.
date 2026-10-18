
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [Header("Gentle Floating")]
    public float floatAmount = 0.12f;
    public float floatSpeed = 1.5f;

    [Header("Gentle Tilt")]
    public float tiltAmount = 4f;
    public float tiltSpeed = 1f;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private float randomOffset;

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        randomOffset = Random.Range(0f, 10f);
    }

    private void Update()
    {
        float verticalOffset =
            Mathf.Sin((Time.time + randomOffset) * floatSpeed)
            * floatAmount;

        transform.position = new Vector3(
            startPosition.x,
            startPosition.y + verticalOffset,
            startPosition.z
        );

        float tilt = Mathf.Sin(
            (Time.time + randomOffset) * tiltSpeed
        ) * tiltAmount;

        transform.rotation =
            startRotation * Quaternion.Euler(0f, 0f, tilt);
    }
}
