using TMPro;
using UnityEngine;

public class RuntimeStatItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statNameText;

    [SerializeField] private TextMeshProUGUI statValueText;

    public void SetData(string statName, string value)
    {
        statNameText.text = statName;
        statValueText.text = value;
    }
}
