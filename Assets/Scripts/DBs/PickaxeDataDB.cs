using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public static class PickaxeDataDB
{
    private static Dictionary<int,PickaxesDataSO> pickaxeDataSOs;
    private static string FilePath = "Sos/PickaxeDatas"; 
    
    public static int GetPickaxeKeyCount()
    {
        return pickaxeDataSOs.Keys.Count;
    }
    public static PickaxesDataSO GetStoneDataSO(int tierKey)
    {
        if (pickaxeDataSOs == null) Initiazlie();
        PickaxesDataSO nowSO = pickaxeDataSOs[tierKey];
        if (nowSO == null)
        {
            Debug.LogWarning($"There is no PickaxesDataSO. tierKey : {tierKey}");
        } 
        return nowSO;
    }
    private static void Initiazlie()
    {
        if (pickaxeDataSOs != null) return;
        pickaxeDataSOs = new();

        PickaxesDataSO[] resources = Resources.LoadAll<PickaxesDataSO>(FilePath);
        for (int i = 0; i < resources.Length; i++)
        {
            PickaxesDataSO nowSO = resources[i];
            if(pickaxeDataSOs.ContainsKey(nowSO.Tier))
            { 
                Debug.LogWarning($"There is PickaxesDataSO tier Duplication. SO Name: {nowSO.name}");
            }
            else
            {
                pickaxeDataSOs.Add(nowSO.Tier, nowSO);
            } 
        }

    }
    
}
