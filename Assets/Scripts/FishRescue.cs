
using UnityEngine;

public class FishRescue : MonoBehaviour
{
    public int scorePoints = 10;
    public GameObject rescueEffectPrefab;

    private bool rescued = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (rescued) return;

        if (other.CompareTag("Player"))
        {
            rescued = true;

            // إضافة السكور
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(scorePoints);
            }

            // إظهار تأثير الإنقاذ عند اللاعب
            if (rescueEffectPrefab != null)
            {
                GameObject effect = Instantiate(
                    rescueEffectPrefab,
                    other.transform.position,
                    Quaternion.identity
                );

                effect.SetActive(true);

                ParticleSystem ps = effect.GetComponent<ParticleSystem>();

                if (ps != null)
                {
                    ps.Play();
                }
                else
                {
                    ParticleSystem[] systems =
                        effect.GetComponentsInChildren<ParticleSystem>(true);

                    foreach (ParticleSystem system in systems)
                    {
                        system.gameObject.SetActive(true);
                        system.Play();
                    }
                }

                Destroy(effect, 5f);
            }

            // حذف السمكة بعد الإنقاذ
            Destroy(gameObject);
        }
    }
}