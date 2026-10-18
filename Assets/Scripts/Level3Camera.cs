
using UnityEngine;

public class Level3Camera : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public SpriteRenderer background;

    [Header("Camera Follow")]
    public float smoothTime = 0.25f;
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    [Header("Dead Zone")]
    public float deadZoneX = 1.5f;
    public float deadZoneY = 1f;

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

        if (cam == null || player == null)
        {
            Debug.LogWarning(
                "Level3Camera: Assign the Camera and Player.");
            enabled = false;
            return;
        }

        if (background == null)
        {
            Debug.LogWarning(
                "Level3Camera: Assign the Background.");
            enabled = false;
            return;
        }

        cam.orthographicSize = normalSize;

        Vector3 startPosition = player.position + offset;
        startPosition.z = offset.z;

        transform.position =
            ClampPositionToBackground(startPosition);

        cameraVelocity = Vector3.zero;
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

        // Keep the camera still until the player leaves
        // the follow zone around the camera center.
        float targetX = currentPosition.x;
        float targetY = currentPosition.y;

        float distanceX =
            player.position.x - currentPosition.x;

        float distanceY =
            player.position.y - currentPosition.y;

        if (distanceX > deadZoneX)
            targetX = player.position.x - deadZoneX;
        else if (distanceX < -deadZoneX)
            targetX = player.position.x + deadZoneX;

        if (distanceY > deadZoneY)
            targetY = player.position.y - deadZoneY;
        else if (distanceY < -deadZoneY)
            targetY = player.position.y + deadZoneY;

        Vector3 targetPosition = new Vector3(
            targetX + offset.x,
            targetY + offset.y,
            offset.z
        );

        targetPosition =
            ClampPositionToBackground(targetPosition);

        Vector3 newPosition = Vector3.SmoothDamp(
            currentPosition,
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

        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        float minX = bounds.min.x + halfWidth;
        float maxX = bounds.max.x - halfWidth;

        float minY = bounds.min.y + halfHeight;
        float maxY = bounds.max.y - halfHeight;

        if (minX > maxX)
            position.x = bounds.center.x;
        else
            position.x = Mathf.Clamp(position.x, minX, maxX);

        if (minY > maxY)
            position.y = bounds.center.y;
        else
            position.y = Mathf.Clamp(position.y, minY, maxY);

        position.z = offset.z;

        return position;
    }

    private void HandleZoom()
    {
        float targetSize = zooming ? zoomSize : normalSize;

        cam.orthographicSize = Mathf.Lerp(
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
        if (background != null)
        {
            Bounds bounds = background.bounds;

            Gizmos.DrawWireCube(
                bounds.center,
                bounds.size
            );
        }
    }
}
