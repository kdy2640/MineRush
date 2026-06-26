using TMPro;
using UnityEngine;

public class CurrentPickaxeInfo : MonoBehaviour
{
    [field: SerializeField] public TextMeshProUGUI DisplayNameText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI MiningPowerText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI MiningSpeedText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI MiningRadiusText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI CriticalChanceText { get; private set; }


}
