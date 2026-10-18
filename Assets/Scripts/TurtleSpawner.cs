
using UnityEngine;
using System.Collections;

public class TurtleSpawner : MonoBehaviour
{
    public GameObject turtlePrefab;

    public float minSpawnDelay = 8f;
    public float maxSpawnDelay = 12f;

    public float spawnX = 7f;
    public float minSpawnY = -3f;
    public float maxSpawnY = 3f;

    private void Start()
    {
        // إظهار أول سلحفاة فور بدء اللعبة
        SpawnTurtle();

        // ثم الاستمرار بإظهار سلاحف جديدة
        StartCoroutine(SpawnMoreTurtles());
    }

    private void SpawnTurtle()
    {
        if (turtlePrefab == null) return;

        Vector2 spawnPosition = new Vector2(
            spawnX,
            Random.Range(minSpawnY, maxSpawnY)
        );

        Instantiate(turtlePrefab, spawnPosition, Quaternion.identity);
    }

    private IEnumerator SpawnMoreTurtles()
    {
        while (true)
        {
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);

            SpawnTurtle();
        }
    }
}
