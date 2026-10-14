
using UnityEngine;

public class SimpleParallax : MonoBehaviour
{
    public Transform player;
    public float parallaxAmount = 0.05f;

    private Vector3 startPosition;
    private Vector3 playerStartPosition;
    private Camera mainCamera;
    private float backgroundZ;

    private void Start()
    {
        mainCamera = Camera.main;

        startPosition = transform.position;
        backgroundZ = transform.position.z;

        if (player != null)
        {
            playerStartPosition = player.position;
        }
    }

    private void LateUpdate()
    {
        if (player == null || mainCamera == null)
            return;

        Vector3 playerMovement =
            player.position - playerStartPosition;

        Vector3 targetPosition = new Vector3(
            startPosition.x + playerMovement.x * parallaxAmount,
            startPosition.y + playerMovement.y * parallaxAmount,
            backgroundZ
        );

        transform.position = targetPosition;
    }
}
