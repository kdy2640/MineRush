using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class RuntimeStatItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statNameText;

    [SerializeField] private TextMeshProUGUI statValueText;

    [SerializeField] private Image iconImage;

    public void SetData(string statName, string value)
    {
        statNameText.text = statName;
        statValueText.text = value;
    }
}
