using UnityEngine;
using TMPro;

public class ResultOreItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI oreNameText;

    [SerializeField] private TextMeshProUGUI amountText;

    public void SetData(OreAmount oreAmount)
    {
        oreNameText.text = oreAmount.oreType.ToString();
        amountText.text = $"x{oreAmount.amount}";
    }
}
