using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class oreDataDB
{
    private static OreDataSO[] oreDataMap;
    private static string FilePath = "Sos/OreDatas"; 

    public static OreDataSO GetOreDataSO(OreType oreType)
    {
        if (oreDataMap == null) Initiazlie();
        OreDataSO nowSO = oreDataMap[(int)oreType];
        if (nowSO == null)
        {
            Debug.LogWarning($"There is no OreData, OreType : {oreType}");
        }

        return nowSO;
    }
    private static void Initiazlie()
    {
        if (oreDataMap != null) return;
        oreDataMap = new OreDataSO[(int)OreType.Length]; 

        OreDataSO[] resources = Resources.LoadAll<OreDataSO>(FilePath);
        for (int i = 0; i < resources.Length; i++)
        {
            OreDataSO nowSO = resources[i]; 

            oreDataMap[(int)nowSO.OreType] = nowSO;
        }

    }

}
