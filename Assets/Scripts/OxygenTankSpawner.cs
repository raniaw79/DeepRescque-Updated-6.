
using UnityEngine;
using System.Collections;

public class OxygenTankSpawner : MonoBehaviour
{
    public GameObject[] oxygenTanks;

    public float timeBetweenTanks = 10f;
    public float timeBeforeDisappear = 10f;

    private void Start()
    {
        foreach (GameObject tank in oxygenTanks)
        {
            if (tank != null)
            {
                tank.SetActive(false);
            }
        }

        StartCoroutine(SpawnTanks());
    }

    private IEnumerator SpawnTanks()
    {
        for (int i = 0; i < oxygenTanks.Length; i++)
        {
            GameObject tank = oxygenTanks[i];

            if (tank != null)
            {
                tank.SetActive(true);

                // مؤقت اختفاء مستقل لهذه الأسطوانة
                StartCoroutine(HideTankAfterDelay(
                    tank, timeBeforeDisappear
                ));
            }

            // موعد ظهور الأسطوانة التالية مستقل
            if (i < oxygenTanks.Length - 1)
            {
                yield return new WaitForSeconds(timeBetweenTanks);
            }
        }
    }

    private IEnumerator HideTankAfterDelay(
        GameObject tank, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (tank != null && tank.activeSelf)
        {
            tank.SetActive(false);
        }
    }
}
