using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI")]
    public TMP_Text scoreText;

    [Header("Level 1 - Complete Panel")]
    public GameObject levelCompletePanel;
    public int levelCompleteScore = 50;

    [Header("Level 2 - Mission Complete Panel")]
    public GameObject difficultyPanel;
    public Button continueButton;
    public int difficultyScore = 50;

    [Header("Next Level")]
    public string nextLevelName = "Level3";

    private int score = 0;
    private bool levelCompleted = false;
    private bool difficultyTriggered = false;

    private void Awake()
    {
        Instance = this;

        if (levelCompletePanel == null)
        {
            GameObject foundPanel =
                GameObject.Find("LevelCompletePanel");

            if (foundPanel != null)
                levelCompletePanel = foundPanel;
        }

        if (difficultyPanel == null)
        {
            GameObject foundPanel =
                GameObject.Find("MissionCompletePanel");

            if (foundPanel != null)
                difficultyPanel = foundPanel;
        }

        Debug.Log("ScoreManager Awake | Scene: " +
                  SceneManager.GetActiveScene().name);
    }

    private void Start()
    {
        Time.timeScale = 1f;

        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);

        if (difficultyPanel != null)
            difficultyPanel.SetActive(false);

        if (continueButton != null)
        {
            continueButton.onClick.RemoveListener(
                ContinueAfterDifficulty);

            continueButton.onClick.AddListener(
                ContinueAfterDifficulty);
        }
        else
        {
            Debug.LogWarning(
                "Continue Button is not assigned in the Inspector!");
        }

        UpdateScoreText();
    }

    public void AddScore(int points)
    {
        string sceneName =
            SceneManager.GetActiveScene().name;

        if (sceneName == "Level1" && levelCompleted)
            return;

        if (sceneName == "Level2" && difficultyTriggered)
            return;

        score += points;
        UpdateScoreText();

        Debug.Log("Current Score: " + score);

        if (sceneName == "Level1" &&
            score >= levelCompleteScore)
        {
            levelCompleted = true;
            ShowLevelCompletePanel();
        }
        else if (sceneName == "Level2" &&
                 score >= difficultyScore)
        {
            difficultyTriggered = true;
            ShowDifficultyPanel();
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    private void ShowLevelCompletePanel()
    {
        if (levelCompletePanel == null)
        {
            Debug.LogError(
                "LevelCompletePanel is not assigned!");
            return;
        }

        levelCompletePanel.SetActive(true);
        Time.timeScale = 0f;

        Debug.Log("LEVEL 1 COMPLETED!");
    }

    private void ShowDifficultyPanel()
    {
        if (difficultyPanel == null)
        {
            Debug.LogError(
                "MissionCompletePanel not found! " +
                "Assign it in the Inspector.");
            return;
        }

        difficultyPanel.SetActive(true);
        Time.timeScale = 0f;

        Debug.Log("MISSION COMPLETE PANEL SHOWN!");
    }

    public void ContinueAfterDifficulty()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextLevelName);
    }
}