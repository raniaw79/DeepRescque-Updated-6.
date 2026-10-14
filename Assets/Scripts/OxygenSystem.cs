using UnityEngine;
using TMPro;

public class OxygenSystem : MonoBehaviour
{
    public float maxOxygen = 100f;
    public float oxygen = 100f;
    public float oxygenDrainRate = 1f;

    public int oxygenTanks = 0;
    public int maxOxygenTanks = 4;

    public TMP_Text oxygenText;
    public GameObject gameOverPanel;
    public GameObject[] hudElements;

    private bool gameOver = false;

    private void Start()
    {
        oxygen = maxOxygen;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        SetHUDVisible(true);
        UpdateOxygenText();
    }

    private void Update()
    {
        if (gameOver)
            return;

        // الخصم الطبيعي المستمر للأكسجين
        oxygen -= oxygenDrainRate * Time.deltaTime;
        oxygen = Mathf.Clamp(oxygen, 0f, maxOxygen);

        if (oxygen <= 0f)
        {
            if (oxygenTanks > 0)
            {
                oxygenTanks--;
                oxygen = maxOxygen;
            }
            else
            {
                UpdateOxygenText();
                GameOver();
                return;
            }
        }

        UpdateOxygenText();
    }

    private void UpdateOxygenText()
    {
        if (oxygenText != null)
        {
            oxygenText.text = Mathf.RoundToInt(oxygen).ToString();
        }
    }

    public void DamageOxygen(float amount)
    {
        if (gameOver)
            return;

        oxygen = Mathf.Max(0f, oxygen - amount);
        UpdateOxygenText();

        Debug.Log("Damage applied: " + amount + " | Remaining: " + oxygen);

        if (oxygen <= 0f)
        {
            if (oxygenTanks > 0)
            {
                oxygenTanks--;
                oxygen = maxOxygen;
                UpdateOxygenText();
            }
            else
            {
                GameOver();
            }
        }
    }

    public void AddOxygenTank()
    {
        if (!gameOver && oxygenTanks < maxOxygenTanks)
        {
            oxygenTanks++;
        }
    }

    public void SharkAttack()
    {
        GameOver();
    }

    public void GameOver()
    {
        if (gameOver)
            return;

        gameOver = true;
        SetHUDVisible(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    private void SetHUDVisible(bool visible)
    {
        if (hudElements == null)
            return;

        foreach (GameObject element in hudElements)
        {
            if (element != null)
                element.SetActive(visible);
        }
    }
}