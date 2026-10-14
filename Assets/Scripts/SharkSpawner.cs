
using UnityEngine;
using System.Collections;

public class SharkSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public float firstSpawnDelay = 10f;
    public float spawnInterval = 3f;

    private SharkMovement[] sharks;
    private Camera mainCamera;
    private int nextSharkIndex;

    private void Start()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("SharkSpawner: Main Camera not found!");
            return;
        }

        // Find all duplicated sharks under SharkSpawner.
        sharks = GetComponentsInChildren<SharkMovement>(true);

        if (sharks.Length == 0)
        {
            Debug.LogError(
                "SharkSpawner: No SharkMovement scripts found!"
            );
            return;
        }

        // Hide all sharks at startup.
        foreach (SharkMovement shark in sharks)
        {
            if (shark == null)
                continue;

            shark.StopMoving();
            shark.gameObject.SetActive(false);
        }

        nextSharkIndex = 0;
        StartCoroutine(SpawnSharks());
    }

    private IEnumerator SpawnSharks()
    {
        yield return new WaitForSeconds(firstSpawnDelay);

        while (true)
        {
            int checkedSharks = 0;

            while (checkedSharks < sharks.Length)
            {
                SharkMovement shark = sharks[nextSharkIndex];

                nextSharkIndex =
                    (nextSharkIndex + 1) % sharks.Length;

                checkedSharks++;

                if (shark == null || shark.gameObject.activeSelf)
                    continue;

                shark.gameObject.SetActive(true);

                int direction = Random.value < 0.5f ? -1 : 1;

                shark.StartMoving(mainCamera, direction);

                break;
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
