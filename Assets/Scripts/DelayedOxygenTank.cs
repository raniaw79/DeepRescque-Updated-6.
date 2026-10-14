using UnityEngine;

public class DelayedOxygenTank : MonoBehaviour
{
    public float appearAfter = 20f;

    private void Start()
    {
        Invoke(nameof(ShowTank), appearAfter);
    }

    private void ShowTank()
    {
        gameObject.SetActive(true);
    }
}