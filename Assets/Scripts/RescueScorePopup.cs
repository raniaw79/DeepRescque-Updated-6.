
using System.Collections;
using TMPro;
using UnityEngine;

public class RescueScorePopup : MonoBehaviour
{
    [Header("Popup Movement")]
    public float moveUpDistance = 60f;
    public float duration = 1f;

    private TMP_Text popupText;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Coroutine animationCoroutine;
    private Color originalColor;
    private bool colorSaved = false;

    private void Awake()
    {
        PrepareComponents();

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }

    private void PrepareComponents()
    {
        if (popupText == null)
            popupText = GetComponent<TMP_Text>();

        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        if (popupText != null && !colorSaved)
        {
            originalColor = popupText.color;

            if (originalColor.a <= 0f)
                originalColor.a = 1f;

            colorSaved = true;
        }
    }

    public void ShowAt(Vector3 worldPosition)
    {
        // تفعيل النص أولًا ثم تجهيز مكوناته
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        PrepareComponents();

        if (popupText == null || rectTransform == null)
        {
            Debug.LogError(
                "Popup: تأكدي أن السكربت على عنصر TextMeshPro نفسه."
            );
            return;
        }

        Canvas canvas = GetComponentInParent<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("Popup: Canvas not found!");
            return;
        }

        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Popup: Main Camera not found!");
            return;
        }

        // وضع النص مباشرة داخل Canvas حتى لا يتأثر بأب مخفي
        if (transform.parent != canvas.transform)
            rectTransform.SetParent(canvas.transform, false);

        Vector3 screenPosition =
            mainCamera.WorldToScreenPoint(worldPosition);

        if (screenPosition.z <= 0f)
            return;

        Camera uiCamera =
            canvas.renderMode == RenderMode.ScreenSpaceOverlay
            ? null
            : (canvas.worldCamera != null
                ? canvas.worldCamera
                : mainCamera);

        RectTransform canvasRect =
            canvas.transform as RectTransform;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPosition,
            uiCamera,
            out Vector2 localPosition))
        {
            return;
        }

        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        rectTransform.anchoredPosition = localPosition;
        rectTransform.localScale = Vector3.one;

        popupText.text = "+20";
        popupText.color = originalColor;
        popupText.enabled = true;

        transform.SetAsLastSibling();
        canvasGroup.alpha = 1f;

        animationCoroutine = StartCoroutine(AnimatePopup());
    }

    private IEnumerator AnimatePopup()
    {
        float timer = 0f;
        Vector2 startPosition = rectTransform.anchoredPosition;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / duration);

            rectTransform.anchoredPosition =
                startPosition +
                Vector2.up * moveUpDistance * progress;

            canvasGroup.alpha = 1f - progress;

            yield return null;
        }

        canvasGroup.alpha = 0f;
        animationCoroutine = null;
    }
}
