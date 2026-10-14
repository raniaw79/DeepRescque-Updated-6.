using UnityEngine;
using TMPro;

public class OxygenTankCounter : MonoBehaviour
{
    public TMP_Text counterText;
    public OxygenSystem oxygenSystem;

    private void Update()
    {
        counterText.text = oxygenSystem.oxygenTanks.ToString();
    }
}