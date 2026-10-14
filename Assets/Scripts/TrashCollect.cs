using UnityEngine;

public class TrashCollect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // كل Trash = +1 Score
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(1);
            }

            // إعادة الـTrash للأعلى
            Vector3 newPosition = transform.position;
            newPosition.y = 6f;
            transform.position = newPosition;
        }
    }
}