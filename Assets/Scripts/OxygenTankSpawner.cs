using UnityEngine;

public class OxygenTankSpawner : MonoBehaviour
{
    public GameObject[] oxygenTanks;

    public float firstTankDelay = 10f;
    public float timeBetweenTanks = 20f;

    private void Start()
    {
        for (int i = 0; i < oxygenTanks.Length; i++)
        {
            oxygenTanks[i].SetActive(false);
        }

        Invoke(nameof(ShowFirstTank), firstTankDelay);
    }

    private void ShowFirstTank()
    {
        if (oxygenTanks.Length > 0)
        {
            oxygenTanks[0].SetActive(true);
            Invoke(nameof(ShowSecondTank), timeBetweenTanks);
        }
    }

    private void ShowSecondTank()
    {
        if (oxygenTanks.Length > 1)
        {
            oxygenTanks[1].SetActive(true);
            Invoke(nameof(ShowThirdTank), timeBetweenTanks);
        }
    }

    private void ShowThirdTank()
    {
        if (oxygenTanks.Length > 2)
        {
            oxygenTanks[2].SetActive(true);
            Invoke(nameof(ShowFourthTank), timeBetweenTanks);
        }
    }

    private void ShowFourthTank()
    {
        if (oxygenTanks.Length > 3)
        {
            oxygenTanks[3].SetActive(true);
        }
    }
}
