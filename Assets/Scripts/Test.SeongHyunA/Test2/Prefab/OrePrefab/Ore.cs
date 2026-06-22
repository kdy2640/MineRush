using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    [Header("Reward")]
    public OreType oreType;

    [Header("XP")]
    public int xpValue;

    //-----------------------------------
    // 전체 광석 자동 등록
    //-----------------------------------

    public static List<Ore> AllOres =
        new List<Ore>();

    private bool mined;

    public bool IsMined => mined;

    private void OnEnable()
    {
        if (!AllOres.Contains(this))
        {
            AllOres.Add(this);
        }
    }

    private void OnDestroy()
    {
        AllOres.Remove(this);
    }

    public void SetMined(bool value)
    {
        mined = value;
    }

    public void ResetOre()
    {
        mined = false;

        gameObject.SetActive(true);

        OreView view =
            GetComponent<OreView>();

        if (view != null)
        {
            view.ResetView();
        }
    }
}