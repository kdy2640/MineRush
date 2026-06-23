using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickaxeShower : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [field: SerializeField] public PickaxesDataSO CurrentPickAxe { get; private set; }

    [SerializeField] private CurrentPickaxeInfo pickaxeInfo;


    public void OnPointerEnter(PointerEventData eventData)
    {
        pickaxeInfo.DisplayNameText.text = CurrentPickAxe.DisplayName;
        pickaxeInfo.MiningPowerText.text = $"Ã¤±¼ °ø°Ý·Â : {CurrentPickAxe.MiningPower}";
        pickaxeInfo.MiningSpeedText.text = $"Ã¤±¼¼Óµµ : {CurrentPickAxe.MiningSpeed}";
        pickaxeInfo.MiningRadiusText.text = $"Ã¤±¼¹Ý°æ : {CurrentPickAxe.MiningRadius}";
        pickaxeInfo.CriticalChanceText.text = $"Å©¸®Æ¼ÄÃ È®·ü : {CurrentPickAxe.CriticalChance}";

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
