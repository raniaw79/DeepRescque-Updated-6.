using UnityEngine;

public class JellyfishMovement : MonoBehaviour
{
    public float moveSpeed = 1.2f;

    public float minX = -7f;
    public float maxX = 7f;

    public float minY = -4f;
    public float maxY = 4f;

    public float changeDirectionTime = 3.5f;

    public float smoothness = 2f;

    private Vector3 targetPosition;
    private Vector3 currentVelocity;

    private float timer;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        ChooseNewTarget();

        // كل Jellyfish يبدأ بوقت مختلف
        timer = Random.Range(0f, changeDirectionTime);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        // حركة ناعمة باتجاه الهدف
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref currentVelocity,
            smoothness
        );

        // قلب الصورة حسب اتجاه الحركة
        if (currentVelocity.x < -0.05f)
        {
            spriteRenderer.flipX = true;
        }
        else if (currentVelocity.x > 0.05f)
        {
            spriteRenderer.flipX = false;
        }

        // اختيار اتجاه جديد
        if (timer >= changeDirectionTime ||
            Vector3.Distance(transform.position, targetPosition) < 0.2f)
        {
            ChooseNewTarget();
            timer = 0f;
        }
    }

    private void ChooseNewTarget()
    {
        float newX = Random.Range(minX, maxX);
        float newY = Random.Range(minY, maxY);

        targetPosition = new Vector3(
            newX,
            newY,
            transform.position.z
        );
    }
}