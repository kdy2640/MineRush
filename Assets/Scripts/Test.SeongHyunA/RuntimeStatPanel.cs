using TMPro;
using UnityEngine;

public class RuntimeStatPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI speedText;
    void Start()
    {
        powerText.text = "Power : 1";
        speedText.text = "Speed : 1";
    }

}
