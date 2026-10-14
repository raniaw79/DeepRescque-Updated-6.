
using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    [Header("Background Bounds")]
    public Transform background;
    public float boundsPadding = 0.5f;

    [Header("Hit Effect")]
    public float knockbackForce = 3f;
    public float knockbackDuration = 0.25f;
    public float shakeAmount = 0.08f;

    private Rigidbody2D rb;
    private Vector2 movement;

    private Vector2 touchStart;
    private bool isDragging;

    private SpriteRenderer spriteRenderer;
    private bool isKnockedBack;
    private Coroutine knockbackCoroutine;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        CalculateBackgroundBounds();
    }

    void CalculateBackgroundBounds()
    {
        if (background == null)
        {
            Debug.LogWarning("Please assign the Background in PlayerMovement.");
            return;
        }

        SpriteRenderer backgroundRenderer =
            background.GetComponent<SpriteRenderer>();

        if (backgroundRenderer != null)
        {
            Bounds bounds = backgroundRenderer.bounds;

            minX = bounds.min.x + boundsPadding;
            maxX = bounds.max.x - boundsPadding;
            minY = bounds.min.y + boundsPadding;
            maxY = bounds.max.y - boundsPadding;
        }
        else
        {
            Debug.LogWarning(
                "Background needs a SpriteRenderer component.");
        }
    }

    void Update()
    {
        if (isKnockedBack)
            return;

        movement = Vector2.zero;

        // Touch على الموبايل
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStart = touch.position;
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Moved ||
                     touch.phase == TouchPhase.Stationary)
            {
                if (isDragging)
                {
                    Vector2 dragDirection =
                        touch.position - touchStart;

                    if (dragDirection.magnitude > 20f)
                        movement = dragDirection.normalized;
                }
            }
            else if (touch.phase == TouchPhase.Ended ||
                     touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
                movement = Vector2.zero;
            }
        }

        // Mouse للاختبار داخل Simulator
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Input.mousePosition;
            isDragging = true;
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector2 dragDirection =
                (Vector2)Input.mousePosition - touchStart;

            if (dragDirection.magnitude > 20f)
                movement = dragDirection.normalized;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            movement = Vector2.zero;
        }

        // اتجاه صورة الغواص
        if (spriteRenderer != null)
        {
            if (movement.x > 0.05f)
                spriteRenderer.flipX = false;
            else if (movement.x < -0.05f)
                spriteRenderer.flipX = true;
        }
    }

    void FixedUpdate()
    {
        if (rb == null || isKnockedBack)
            return;

        Vector2 nextPosition =
            rb.position + movement * speed * Time.fixedDeltaTime;

        if (background != null)
        {
            nextPosition.x = Mathf.Clamp(
                nextPosition.x, minX, maxX);

            nextPosition.y = Mathf.Clamp(
                nextPosition.y, minY, maxY);
        }

        rb.MovePosition(nextPosition);
    }

    public void TakeHit(Vector2 hazardPosition)
    {
        if (isKnockedBack)
            return;

        if (knockbackCoroutine != null)
            StopCoroutine(knockbackCoroutine);

        knockbackCoroutine =
            StartCoroutine(KnockbackEffect(hazardPosition));
    }

    IEnumerator KnockbackEffect(Vector2 hazardPosition)
    {
        isKnockedBack = true;

        Vector2 direction =
            (Vector2)transform.position - hazardPosition;

        if (direction.sqrMagnitude < 0.01f)
            direction = Vector2.up;

        direction.Normalize();

        rb.linearVelocity = direction * knockbackForce;

        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < knockbackDuration)
        {
            float shakeX =
                Random.Range(-shakeAmount, shakeAmount);

            float shakeY =
                Random.Range(-shakeAmount, shakeAmount);

            transform.localPosition =
                originalPosition + new Vector3(shakeX, shakeY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
        rb.linearVelocity = Vector2.zero;
        movement = Vector2.zero;
        isKnockedBack = false;
        knockbackCoroutine = null;
    }
}
