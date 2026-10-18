
using UnityEngine;

public class JellyfishMovement : MonoBehaviour
{
    [Header("Speed")]
    public float minSpeed = 0.7f;
    public float maxSpeed = 3.5f;
    public float acceleration = 2.5f;

    [Header("Movement")]
    public float smoothness = 1.5f;
    public float changeDirectionTime = 3f;

    [Header("Background Bounds")]
    public SpriteRenderer background;
    public float edgePadding = 0.5f;

    private Vector3 targetPosition;
    private Vector3 currentVelocity;

    private float timer;
    private float currentSpeed;
    private float targetSpeed;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (background == null)
        {
            Debug.LogError(
                "Lionfish: Please assign the Level3 background!"
            );

            enabled = false;
            return;
        }

        currentSpeed = Random.Range(minSpeed, maxSpeed);
        targetSpeed = Random.Range(minSpeed, maxSpeed);

        ChooseNewTarget();

        timer = Random.Range(0f, changeDirectionTime);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        // Accelerate or slow down smoothly
        currentSpeed = Mathf.MoveTowards(
            currentSpeed,
            targetSpeed,
            acceleration * Time.deltaTime
        );

        // Move smoothly toward the target
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref currentVelocity,
            smoothness,
            currentSpeed
        );

        // Flip the fish according to horizontal movement
        if (spriteRenderer != null)
        {
            if (currentVelocity.x < -0.05f)
            {
                spriteRenderer.flipX = true;
            }
            else if (currentVelocity.x > 0.05f)
            {
                spriteRenderer.flipX = false;
            }
        }

        // Choose a new target and speed
        if (timer >= changeDirectionTime ||
            Vector3.Distance(transform.position, targetPosition) < 0.15f)
        {
            ChooseNewTarget();

            targetSpeed = Random.Range(minSpeed, maxSpeed);

            // 25% chance of a fast swimming burst
            if (Random.value < 0.25f)
            {
                targetSpeed = maxSpeed;
            }

            timer = 0f;
        }
    }

    private void ChooseNewTarget()
    {
        Bounds bounds = background.bounds;

        float halfWidth = 0f;
        float halfHeight = 0f;

        if (spriteRenderer != null)
        {
            halfWidth = spriteRenderer.bounds.extents.x;
            halfHeight = spriteRenderer.bounds.extents.y;
        }

        float minX = bounds.min.x + halfWidth + edgePadding;
        float maxX = bounds.max.x - halfWidth - edgePadding;

        float minY = bounds.min.y + halfHeight + edgePadding;
        float maxY = bounds.max.y - halfHeight - edgePadding;

        if (minX >= maxX || minY >= maxY)
        {
            Debug.LogWarning(
                "Lionfish: Check the background size and edge padding!"
            );

            return;
        }

        targetPosition = new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY),
            transform.position.z
        );
    }
}
