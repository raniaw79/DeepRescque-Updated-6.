using UnityEngine;

public class Level3Camera : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public SpriteRenderer background;

    [Header("Camera Follow")]
    [Range(0.05f, 1f)]
    public float smoothTime = 0.25f;

    public Vector3 offset = new Vector3(0f, 0f, -10f);

    [Header("Dead Zone")]
    public float deadZoneX = 2f;
    public float deadZoneY = 1.5f;

    [Header("Zoom")]
    public float normalSize = 4.6f;
    public float zoomSize = 3.5f;
    public float zoomSpeed = 3f;

    private Camera cam;
    private Vector3 cameraVelocity;
    private bool zooming = false;

    private void Start()
    {
        cam = GetComponent<Camera>();

        if (cam == null)
        {
            enabled = false;
            return;
        }

        if (player == null)
        {
            enabled = false;
            return;
        }

        if (background == null)
        {
            enabled = false;
            return;
        }

        cam.orthographicSize = normalSize;

        Vector3 startPosition = transform.position;
        startPosition.z = offset.z;

        transform.position = ClampPositionToBackground(startPosition);
    }

    private void LateUpdate()
    {
        if (player == null || background == null || cam == null)
            return;

        HandleZoom();

        FollowPlayer();
    }

    private void FollowPlayer()
    {
        Vector3 currentPosition = transform.position;

        float targetX = currentPosition.x;
        float targetY = currentPosition.y;

        float horizontalDistance =
            player.position.x - currentPosition.x;

        float verticalDistance =
            player.position.y - currentPosition.y;

        if (horizontalDistance > deadZoneX)
        {
            targetX =
                player.position.x - deadZoneX;
        }
        else if (horizontalDistance < -deadZoneX)
        {
            targetX =
                player.position.x + deadZoneX;
        }

        if (verticalDistance > deadZoneY)
        {
            targetY =
                player.position.y - deadZoneY;
        }
        else if (verticalDistance < -deadZoneY)
        {
            targetY =
                player.position.y + deadZoneY;
        }

        targetX += offset.x;
        targetY += offset.y;

        Vector3 targetPosition = new Vector3(
            targetX,
            targetY,
            offset.z
        );

        targetPosition =
            ClampPositionToBackground(targetPosition);

        Vector3 newPosition =
            Vector3.SmoothDamp(
                transform.position,
                targetPosition,
                ref cameraVelocity,
                smoothTime
            );

        newPosition =
            ClampPositionToBackground(newPosition);

        newPosition.z = offset.z;

        transform.position = newPosition;
    }

    private Vector3 ClampPositionToBackground(
        Vector3 position)
    {
        Bounds bounds = background.bounds;

        float cameraHalfHeight =
            cam.orthographicSize;

        float cameraHalfWidth =
            cameraHalfHeight * cam.aspect;

        float minX =
            bounds.min.x + cameraHalfWidth;

        float maxX =
            bounds.max.x - cameraHalfWidth;

        float minY =
            bounds.min.y + cameraHalfHeight;

        float maxY =
            bounds.max.y - cameraHalfHeight;

        if (minX > maxX)
        {
            position.x =
                bounds.center.x;
        }
        else
        {
            position.x =
                Mathf.Clamp(
                    position.x,
                    minX,
                    maxX
                );
        }

        if (minY > maxY)
        {
            position.y =
                bounds.center.y;
        }
        else
        {
            position.y =
                Mathf.Clamp(
                    position.y,
                    minY,
                    maxY
                );
        }

        position.z = offset.z;

        return position;
    }

    private void HandleZoom()
    {
        float targetSize =
            zooming
                ? zoomSize
                : normalSize;

        cam.orthographicSize =
            Mathf.Lerp(
                cam.orthographicSize,
                targetSize,
                zoomSpeed * Time.deltaTime
            );
    }

    public void ZoomIn()
    {
        zooming = true;
    }

    public void ZoomOut()
    {
        zooming = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (background == null)
            return;

        Bounds bounds = background.bounds;

        Gizmos.DrawWireCube(
            bounds.center,
            bounds.size
        );

        Gizmos.DrawWireCube(
            new Vector3(
                transform.position.x,
                transform.position.y,
                0f
            ),
            new Vector3(
                deadZoneX * 2f,
                deadZoneY * 2f,
                0f
            )
        );
    }
}