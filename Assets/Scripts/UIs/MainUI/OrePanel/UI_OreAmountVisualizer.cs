using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UI_OreAmountVisualizer : MonoBehaviour
{
    [SerializeField] private Image BackGround;
    [SerializeField] private Image OreImage;
    [SerializeField] private TextMeshProUGUI OreCount;
     

    public void SetOre(OreAmount amount)
    {
        OreImage.sprite = OreDataDB.GetOreDataSO(amount.oreType).OreSprite;
        OreCount.text = amount.amount.ToString();
    }
    public void Clear()
    { 
        OreCount.text = "";
    } 
}
