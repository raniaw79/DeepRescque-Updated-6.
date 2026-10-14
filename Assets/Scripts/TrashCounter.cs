using UnityEngine;
using TMPro;

public class TrashCounter : MonoBehaviour
{
    public static TrashCounter Instance;

    public TMP_Text counterText;
    public GameObject levelCompletePanel;

    public int trashTarget = 50;

    private int trashCollected = 0;
    private bool levelCompleted = false;

    private void Awake()
    {
        Instance = this;

        // البحث عن LevelCompletePanel تلقائيًا
        // حتى لو كان غير مفعّل
        if (levelCompletePanel == null)
        {
            Canvas canvas = FindFirstObjectByType<Canvas>();

            if (canvas != null)
            {
                Transform panel = FindChildRecursive(
                    canvas.transform,
                    "LevelCompletePanel"
                );

                if (panel != null)
                {
                    levelCompletePanel = panel.gameObject;
                }
            }
        }
    }

    private void Start()
    {
        UpdateCounter();

        // إذا كان الـPanel موجود، نخفيه في بداية اللعبة
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(false);
        }
    }

    public void AddTrash()
    {
        // إذا اكتمل المستوى، لا نزيد العداد
        if (levelCompleted)
        {
            return;
        }

        trashCollected++;

        UpdateCounter();

        // كل 10 زبالة = 10 نقاط
        if (trashCollected % 10 == 0)
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(10);
            }
        }

        // زيادة سرعة الزبالة
        FallingTrash[] allTrash =
            FindObjectsByType<FallingTrash>(FindObjectsSortMode.None);

        foreach (FallingTrash trash in allTrash)
        {
            trash.IncreaseSpeed(0.1f);
        }

        // عند الوصول إلى 50 زبالة
        if (trashCollected >= trashTarget)
        {
            levelCompleted = true;
            ShowLevelComplete();
        }
    }

    private void ShowLevelComplete()
    {
        // إذا كان الـPanel موجود
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);

            // إيقاف اللعبة
            Time.timeScale = 0f;

            Debug.Log("LEVEL 2 POPUP SHOWN!");
        }
    }

    private void UpdateCounter()
    {
        if (counterText != null)
        {
            counterText.text = ": " + trashCollected;
        }
    }

    // البحث عن عنصر داخل الـCanvas حتى لو كان Inactive
    private Transform FindChildRecursive(
        Transform parent,
        string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
            {
                return child;
            }

            Transform result = FindChildRecursive(
                child,
                childName
            );

            if (result != null)
            {
                return result;
            }
        }

        return null;
    }
}