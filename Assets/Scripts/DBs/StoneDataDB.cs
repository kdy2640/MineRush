using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class StoneDataDB
{
    private static StoneDataSO[][] StoneDataMap;
    private static string FilePath = "Sos/StoneDatas";
    public enum StoneType
    {
        Base,
        Fragment,
        Pure,
        Length
    }
    
    public static StoneDataSO GetStoneDataSO(StoneType stoneType, OreType oreType)
    {
        if (StoneDataMap == null) Initiazlie();
        StoneDataSO nowSO = StoneDataMap[(int)stoneType][(int)oreType];
        if (nowSO == null)
        {
            Debug.LogWarning($"There is no StoneData. StoneType : {stoneType}, OreType : {oreType}");
        }

        return nowSO;
    }
    private static void Initiazlie()
    {
        if (StoneDataMap != null) return;
        StoneDataMap = new StoneDataSO[(int)StoneType.Length][];
        for (int i = 0; i < (int)StoneType.Length; i++)
        {
            StoneDataMap[i] = new StoneDataSO[(int)OreType.Length];
        }
        StoneDataSO[] resources = Resources.LoadAll<StoneDataSO>(FilePath);
        for (int i = 0; i < resources.Length; i++)
        {
            StoneDataSO nowSO =  resources[i];
            if (nowSO.StoneType == StoneType.Base)
            {
                for (int j = 0; j < (int)OreType.Length; j++)
                {
                    StoneDataMap[(int)StoneType.Base][j] = nowSO;
                }
                continue;
            }
            StoneDataMap[(int)nowSO.StoneType][(int)nowSO.OreType] = nowSO;
        }

    }
    
}
