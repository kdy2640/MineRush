using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatItemUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI amountText;

    public void SetStatItemUI(Sprite sprite, string oreName, int amount)
    {
        if(icon != null) icon.sprite = sprite;
        nameText.text = oreName;
        amountText.text = $"+ {amount}";
    }
}
