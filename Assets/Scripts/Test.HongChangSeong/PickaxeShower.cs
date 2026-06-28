using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickaxeShower : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //띄울 SO
    [field: SerializeField] public PickaxesDataSO CurrentPickAxe { get; private set; }

    //띄울 장소
    [SerializeField] private PickaxeInfoPanel pickaxeInfo;


    public void OnPointerEnter(PointerEventData eventData) //일반 메서드로 변환한 다음, 매개변수로 받는 식으로 바꿔야 할지.
    {
        pickaxeInfo.DisplayNameText.text = CurrentPickAxe.DisplayName;
        pickaxeInfo.MiningPowerText.text = $"채굴 공격력 : {CurrentPickAxe.MiningPower}";
        pickaxeInfo.MiningSpeedText.text = $"채굴속도 : {CurrentPickAxe.MiningSpeed}";
        pickaxeInfo.MiningRadiusText.text = $"채굴반경 : {CurrentPickAxe.MiningRadius}";
        pickaxeInfo.CriticalChanceText.text = $"크리티컬 확률 : {CurrentPickAxe.CriticalChance}";

    }

    public void OnPointerExit(PointerEventData eventdata)
    {
        pickaxeInfo.DisplayNameText.text = null;
        pickaxeInfo.MiningPowerText.text = null;
        pickaxeInfo.MiningSpeedText.text = null;
        pickaxeInfo.MiningRadiusText.text = null;
        pickaxeInfo.CriticalChanceText.text = null;
    }

}
