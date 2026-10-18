
using System.Collections;
using UnityEngine;

public class TreasureWaveManager : MonoBehaviour
{
    [Header("Treasure Alert")]
    public GameObject alertPanel;
    public float alertDelay = 20f;
    public float alertDuration = 3f;

    [Header("Treasures")]
    public GameObject[] treasures;

    [Tooltip("Seconds between each treasure appearance")]
    public float spawnInterval = 25f;

    private Vector3[] originalPositions;

    private void Awake()
    {
        // حفظ المواقع التي حددتيها للكنوز داخل Unity
        if (treasures != null)
        {
            originalPositions = new Vector3[treasures.Length];

            for (int i = 0; i < treasures.Length; i++)
            {
                if (treasures[i] != null)
                {
                    originalPositions[i] =
                        treasures[i].transform.position;
                }
            }
        }
    }

    private void Start()
    {
        Time.timeScale = 1f;

        // إخفاء رسالة الكنوز بالبداية
        if (alertPanel != null)
        {
            alertPanel.SetActive(false);
        }

        // إخفاء الكنوز مع الاحتفاظ بمواقعها
        if (treasures != null)
        {
            foreach (GameObject treasure in treasures)
            {
                if (treasure != null)
                {
                    treasure.SetActive(false);
                }
            }
        }

        StartCoroutine(SpawnTreasureWave());
    }

    private IEnumerator SpawnTreasureWave()
    {
        // الانتظار قبل ظهور رسالة الكنوز
        yield return new WaitForSeconds(alertDelay);

        // إظهار الرسالة وتجميد اللعبة مؤقتًا
        if (alertPanel != null)
        {
            alertPanel.SetActive(true);
            Time.timeScale = 0f;

            // الانتظار بالوقت الحقيقي حتى أثناء تجميد اللعبة
            yield return new WaitForSecondsRealtime(alertDuration);

            alertPanel.SetActive(false);
            Time.timeScale = 1f;
        }

        if (treasures == null || originalPositions == null)
        {
            yield break;
        }

        // إظهار الكنوز بالتتابع في مواقعها الأصلية
        for (int i = 0; i < treasures.Length; i++)
        {
            GameObject treasure = treasures[i];

            if (treasure == null)
            {
                continue;
            }

            // إعادة الكنز إلى الموقع المحدد في Unity
            treasure.transform.position = originalPositions[i];

            // إظهار الكنز الحالي دون إخفاء الكنوز السابقة
            treasure.SetActive(true);

            Debug.Log("Treasure " + (i + 1) + " appeared.");

            // الانتظار 25 ثانية قبل الكنز التالي
            if (i < treasures.Length - 1)
            {
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }

    private void OnDisable()
    {
        // منع بقاء اللعبة متجمدة
        Time.timeScale = 1f;
    }
}
