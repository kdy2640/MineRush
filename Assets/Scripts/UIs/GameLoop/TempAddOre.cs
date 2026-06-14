
using System.Collections.Generic;
using UnityEngine;

public class TempAddOre : MonoBehaviour
{
    GameManager manager;
    void Start()
    {
        manager = GameManager.Instance;
    }

    public void OnButtonClicked()
    {
        List<OreAmount> rewards = new List<OreAmount>() { new OreAmount(OreType.Copper, 2), new OreAmount(OreType.Iron, 1) };
        manager.OreManager.AddRange(rewards); 
    }

}
