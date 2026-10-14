
using UnityEngine;

public class SharkMovement : MonoBehaviour
{
    [Header("Swimming Speed")]
    public float minSpeed = 1.5f;
    public float maxSpeed = 2.5f;
    public float burstSpeed = 3f;
    public float acceleration = 1.5f;

    [Header("Player Chase")]
    public Transform player;
    public float detectionRange = 1.5f;
    public float stopChasingRange = 4f;
    public float chaseSpeed = 2.5f;
    public float chaseTurnSpeed = 1.5f;

    [Header("Direction Changes")]
    public float minTurnTime = 3f;
    public float maxTurnTime = 5f;
    public float turnSmoothness = 1f;

    [Header("Natural Swimming")]
    public float waveHeight = 0.1f;
    public float waveSpeed = 1f;

    [Header("Screen")]
    public float screenPadding = 1f;

    [Range(0f, 1f)]
    public float exitChance = 0.1f;

    [Header("Sprite Direction")]
    public bool spriteFacesRight = true;

    private Camera mainCamera;
    private SpriteRenderer sharkSprite;

    private Vector2 moveDirection;
    private Vector2 targetDirection;

    private float currentSpeed;
    private float targetSpeed;
    private float turnTimer;
    private float nextTurnTime;
    private float waveOffset;
    private float baseY;

    private bool canMove;
    private bool exiting;
    private bool enteredScreen;
    private bool chasingPlayer;

    private void Awake()
    {
        sharkSprite = GetComponentInChildren<SpriteRenderer>(true);
    }

    public void StartMoving()
    {
        StartMoving(
            Camera.main,
            Random.value < 0.5f ? -1 : 1
        );
    }

    public void StartMoving(Camera cam, int entryDirection)
    {
        mainCamera = cam;

        if (mainCamera == null)
        {
            Debug.LogError("SharkMovement: Main Camera not found!");
            return;
        }

        if (sharkSprite == null)
            sharkSprite = GetComponentInChildren<SpriteRenderer>(true);

        FindPlayer();

        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = halfHeight * mainCamera.aspect;

        Vector3 camPos = mainCamera.transform.position;

        float startX = entryDirection > 0
            ? camPos.x - halfWidth - screenPadding
            : camPos.x + halfWidth + screenPadding;

        baseY = Random.Range(
            camPos.y - halfHeight * 0.5f,
            camPos.y + halfHeight * 0.5f
        );

        transform.position = new Vector3(
            startX, baseY, transform.position.z
        );

        waveOffset = Random.Range(0f, 100f);

        moveDirection = new Vector2(
            entryDirection,
            Random.Range(-0.05f, 0.05f)
        ).normalized;

        targetDirection = moveDirection;

        currentSpeed = Random.Range(minSpeed, maxSpeed);
        targetSpeed = currentSpeed;

        turnTimer = 0f;
        nextTurnTime = Random.Range(minTurnTime, maxTurnTime);

        exiting = false;
        enteredScreen = false;
        chasingPlayer = false;
        canMove = true;

        UpdateFacing();
    }

    private void Update()
    {
        if (!canMove || mainCamera == null)
            return;

        FindPlayer();

        if (!enteredScreen)
        {
            Swim();
            CheckScreenBounds();
            UpdateFacing();
            return;
        }

        UpdatePlayerChase();

        turnTimer += Time.deltaTime;

        if (!chasingPlayer && !exiting &&
            turnTimer >= nextTurnTime)
        {
            ChooseNewDirection();
        }

        Swim();
        UpdateFacing();
        CheckScreenBounds();
    }

    private void Swim()
    {
        currentSpeed = Mathf.MoveTowards(
            currentSpeed,
            targetSpeed,
            acceleration * Time.deltaTime
        );

        float smoothTurn = chasingPlayer
            ? chaseTurnSpeed
            : turnSmoothness;

        moveDirection = Vector2.Lerp(
            moveDirection,
            targetDirection,
            Mathf.Clamp01(smoothTurn * Time.deltaTime)
        ).normalized;

        Vector3 position = transform.position;

        position += (Vector3)(
            moveDirection * currentSpeed * Time.deltaTime
        );

        float wave = Mathf.Sin(
            Time.time * waveSpeed + waveOffset
        ) * waveHeight;

        position.y += wave * Time.deltaTime;
        transform.position = position;
    }

    private void FindPlayer()
    {
        if (player != null && player.gameObject.activeInHierarchy)
            return;

        player = null;

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;
    }

    private void UpdatePlayerChase()
    {
        if (player == null || exiting)
            return;

        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        if (!chasingPlayer && distance <= detectionRange)
        {
            chasingPlayer = true;
            targetSpeed = chaseSpeed;
        }

        if (chasingPlayer && distance >= stopChasingRange)
        {
            chasingPlayer = false;
            ChooseNewDirection();
            return;
        }

        if (chasingPlayer)
        {
            Vector2 toPlayer =
                (Vector2)player.position -
                (Vector2)transform.position;

            if (toPlayer.sqrMagnitude > 0.001f)
            {
                toPlayer.y = Mathf.Clamp(toPlayer.y, -1.5f, 1.5f);
                targetDirection = toPlayer.normalized;
            }

            targetSpeed = chaseSpeed;
        }
    }

    private void ChooseNewDirection()
    {
        turnTimer = 0f;

        nextTurnTime = Random.Range(
            minTurnTime, maxTurnTime
        );

        targetSpeed = Random.value < 0.2f
            ? burstSpeed
            : Random.Range(minSpeed, maxSpeed);

        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = halfHeight * mainCamera.aspect;

        Vector3 camPos = mainCamera.transform.position;

        if (Random.value < exitChance)
        {
            exiting = true;
            chasingPlayer = false;

            float exitX = Random.value < 0.5f
                ? camPos.x - halfWidth - screenPadding
                : camPos.x + halfWidth + screenPadding;

            float exitY = Random.Range(
                camPos.y - halfHeight * 0.5f,
                camPos.y + halfHeight * 0.5f
            );

            Vector2 exitTarget = new Vector2(exitX, exitY);

            targetDirection = (
                exitTarget - (Vector2)transform.position
            ).normalized;

            targetSpeed = Random.Range(minSpeed, maxSpeed);
            return;
        }

        exiting = false;

        float targetX = Random.Range(
            camPos.x - halfWidth * 0.7f,
            camPos.x + halfWidth * 0.7f
        );

        float targetY = Random.Range(
            camPos.y - halfHeight * 0.35f,
            camPos.y + halfHeight * 0.35f
        );

        Vector2 destination = new Vector2(targetX, targetY);
        Vector2 direction = destination -
            (Vector2)transform.position;

        direction.y = Mathf.Clamp(direction.y, -1.5f, 1.5f);

        if (direction.sqrMagnitude > 0.001f)
            targetDirection = direction.normalized;
    }

    private void CheckScreenBounds()
    {
        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = halfHeight * mainCamera.aspect;

        Vector3 camPos = mainCamera.transform.position;
        Vector3 pos = transform.position;

        float spriteWidth = sharkSprite != null
            ? sharkSprite.bounds.extents.x
            : 0.5f;

        if (!enteredScreen)
        {
            if (pos.x >= camPos.x - halfWidth &&
                pos.x <= camPos.x + halfWidth &&
                pos.y >= camPos.y - halfHeight &&
                pos.y <= camPos.y + halfHeight)
            {
                enteredScreen = true;
                baseY = pos.y;
            }

            return;
        }

        bool outside =
            pos.x < camPos.x - halfWidth - spriteWidth - screenPadding ||
            pos.x > camPos.x + halfWidth + spriteWidth + screenPadding ||
            pos.y < camPos.y - halfHeight - screenPadding ||
            pos.y > camPos.y + halfHeight + screenPadding;

        if (outside)
        {
            canMove = false;
            gameObject.SetActive(false);
        }
    }

    private void UpdateFacing()
    {
        if (sharkSprite == null)
            return;

        if (Mathf.Abs(moveDirection.x) > 0.05f)
        {
            bool movingRight = moveDirection.x > 0f;

            sharkSprite.flipX =
                movingRight != spriteFacesRight;
        }
    }

    public void StopMoving()
    {
        canMove = false;
        currentSpeed = 0f;
        targetSpeed = 0f;
        chasingPlayer = false;
    }
}
