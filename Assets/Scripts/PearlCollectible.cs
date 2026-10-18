
using System.Collections;
using UnityEngine;

public class PearlCollectible : MonoBehaviour
{
    [Header("Score")]
    public int scorePoints = 5;

    [Header("Different Falling Speeds")]
    public float minFallSpeed = 1f;
    public float maxFallSpeed = 2.5f;

    [Header("Swimming Movement")]
    public float swayAmount = 0.4f;
    public float minSwaySpeed = 0.8f;
    public float maxSwaySpeed = 1.8f;

    [Header("Different Start Times")]
    public float minStartDelay = 0f;
    public float maxStartDelay = 6f;

    [Header("Respawn")]
    public float minRespawnDelay = 0.5f;
    public float maxRespawnDelay = 2f;

    [Header("Screen Position")]
    public float horizontalPadding = 0.4f;
    public float topPadding = 0.3f;
    public float bottomPadding = 0.7f;

    private SpriteRenderer pearlSprite;
    private Collider2D pearlCollider;
    private Camera mainCamera;

    private float fallSpeed;
    private float swaySpeed;
    private float swayOffset;

    private float currentY;
    private float baseX;

    private bool collected = false;
    private bool isMoving = false;
    private bool isRespawning = false;

    private void Awake()
    {
        pearlSprite = GetComponent<SpriteRenderer>();
        pearlCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("PearlCollectible: Main Camera not found!");
            enabled = false;
            return;
        }

        // إخفاء الصدفة حتى يحين وقت نزولها
        SetVisible(false);

        // بدء الصدف بأوقات مختلفة
        StartCoroutine(StartFalling());
    }

    private IEnumerator StartFalling()
    {
        float delay = Random.Range(
            minStartDelay,
            maxStartDelay
        );

        yield return new WaitForSeconds(delay);

        if (collected)
            yield break;

        ResetAtTop();
        SetVisible(true);
        isMoving = true;
    }

    private void Update()
    {
        if (collected || !isMoving || isRespawning)
            return;

        // النزول إلى الأسفل
        currentY -= fallSpeed * Time.deltaTime;

        // حركة ناعمة يمينًا وشمالًا
        float sidewaysMovement =
            Mathf.Sin(Time.time * swaySpeed + swayOffset)
            * swayAmount;

        transform.position = new Vector3(
            baseX + sidewaysMovement,
            currentY,
            transform.position.z
        );

        // معرفة مكان أسفل الشاشة
        float bottomY = GetBottomScreenY();

        // إذا وصلت الصدفة إلى الأسفل، نعيدها من الأعلى
        if (currentY < bottomY)
        {
            StartCoroutine(RespawnPearl());
        }
    }

    private IEnumerator RespawnPearl()
    {
        isRespawning = true;
        isMoving = false;

        // إخفاء مؤقت، وليس حذف الصدفة
        SetVisible(false);

        float delay = Random.Range(
            minRespawnDelay,
            maxRespawnDelay
        );

        yield return new WaitForSeconds(delay);

        if (collected)
            yield break;

        ResetAtTop();

        SetVisible(true);
        isMoving = true;
        isRespawning = false;
    }

    private void ResetAtTop()
    {
        float leftX;
        float rightX;
        float topY;
        float bottomY;

        GetScreenBounds(
            out leftX,
            out rightX,
            out topY,
            out bottomY
        );

        // اختيار موقع مختلف على عرض الشاشة
        if (rightX < leftX)
        {
            float centerX = (leftX + rightX) / 2f;
            leftX = centerX;
            rightX = centerX;
        }

        baseX = Random.Range(leftX, rightX);
        currentY = topY;

        // سرعة وميل حركة مختلفان لكل نزلة
        fallSpeed = Random.Range(
            minFallSpeed,
            maxFallSpeed
        );

        swaySpeed = Random.Range(
            minSwaySpeed,
            maxSwaySpeed
        );

        swayOffset = Random.Range(0f, Mathf.PI * 2f);

        transform.position = new Vector3(
            baseX,
            currentY,
            transform.position.z
        );
    }

    private void GetScreenBounds(
        out float leftX,
        out float rightX,
        out float topY,
        out float bottomY
    )
    {
        float depth = Mathf.Abs(
            transform.position.z - mainCamera.transform.position.z
        );

        Vector3 leftBottom = mainCamera.ViewportToWorldPoint(
            new Vector3(0f, 0f, depth)
        );

        Vector3 rightTop = mainCamera.ViewportToWorldPoint(
            new Vector3(1f, 1f, depth)
        );

        leftX = leftBottom.x + horizontalPadding;
        rightX = rightTop.x - horizontalPadding;

        topY = rightTop.y + topPadding;
        bottomY = leftBottom.y - bottomPadding;
    }

    private float GetBottomScreenY()
    {
        float depth = Mathf.Abs(
            transform.position.z - mainCamera.transform.position.z
        );

        return mainCamera.ViewportToWorldPoint(
            new Vector3(0.5f, 0f, depth)
        ).y - bottomPadding;
    }

    private void SetVisible(bool visible)
    {
        if (pearlSprite != null)
            pearlSprite.enabled = visible;

        if (pearlCollider != null)
            pearlCollider.enabled = visible;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected || !other.CompareTag("Player"))
            return;

        collected = true;
        isMoving = false;

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scorePoints);
        }

        // إذا جمعها الغواص، تختفي نهائيًا
        Destroy(gameObject);
    }
}
