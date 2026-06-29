using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultOreItem : MonoBehaviour
{
    [Header("[ UI References ]")]
    [SerializeField] private Image oreIcon;
    [SerializeField] private TextMeshProUGUI oreNameText;
    [SerializeField] private TextMeshProUGUI amountText;

    [Header("[ Ore Icons ]")]
    [SerializeField] private Sprite copperSprite;
    [SerializeField] private Sprite ironSprite;
    [SerializeField] private Sprite goldSprite;
    [SerializeField] private Sprite diamondSprite;
    public void SetData(OreAmount oreAmount)
    {
            oreNameText.text = oreAmount.oreType.ToString();
            amountText.text = $"x{oreAmount.amount}";

        switch (oreAmount.oreType)
            {
            case OreType.Copper:
                oreIcon.color = new Color(0.8f, 0.5f, 0.2f); break;

            case OreType.Iron:
                oreIcon.color = Color.gray; break;

            case OreType.Gold:
                oreIcon.color = Color.yellow; break;

            case OreType.Diamond:
                oreIcon.color = Color.cyan; break;
            }
    }
    //수량 텍스트만 변경
    public void SetAmount(int amount)
    {
        amountText.text = $"X{amount}";
    }
    }