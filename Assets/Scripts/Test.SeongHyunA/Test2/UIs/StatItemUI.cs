using TMPro;
using UnityEngine;

public class StatItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI oreNameText;
    [SerializeField] private TextMeshProUGUI sessionText;
    [SerializeField] private TextMeshProUGUI totalText;

    public void SetData(string oreName, int sessionAmount, int totalAmount)
    {
        if (oreNameText != null)
            oreNameText.text = oreName;

        if (sessionText != null)
            sessionText.text = $"Session : {sessionAmount}";

        if (totalText != null)
            totalText.text = $"Total : {totalAmount}";
    }
}