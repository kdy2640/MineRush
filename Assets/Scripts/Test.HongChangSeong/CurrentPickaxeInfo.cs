using TMPro;
using UnityEngine;

public class CurrentPickaxeInfo : MonoBehaviour
{
    [field: SerializeField] public TextMeshProUGUI DisplayNameText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI MiningPowerText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI MiningSpeedText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI MiningRadiusText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI CriticalChanceText { get; private set; }


    //전달받은 데이터를 실제로 전시하는 역할.
    public void ShowPickaxeInfo(PickaxesDataSO currentPickaxe)
    {
        DisplayNameText.text = currentPickaxe.DisplayName;
        MiningPowerText.text = $"채굴 공격력 : {currentPickaxe.MiningPower}";
        MiningSpeedText.text = $"채굴속도 : {currentPickaxe.MiningSpeed}";
        MiningRadiusText.text = $"채굴반경 : {currentPickaxe.MiningRadius}";
        CriticalChanceText.text = $"크리티컬 확률 : {currentPickaxe.CriticalChance}";
    }
}
