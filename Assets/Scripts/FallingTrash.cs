using UnityEngine;

public class FallingTrash : MonoBehaviour
{
    public float fallSpeed = 1.5f;
    public float bottomY = -6f;
    public float topY = 6f;

    private void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        if (transform.position.y <= bottomY)
        {
            Vector3 newPosition = transform.position;
            newPosition.y = topY;
            newPosition.x = Random.Range(-7f, 7f);
            transform.position = newPosition;
        }
    }

    public void IncreaseSpeed(float amount)
    {
        fallSpeed += amount;
    }
}