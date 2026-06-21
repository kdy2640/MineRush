using UnityEngine;

public class OreResetSystem : MonoBehaviour
{
    public void ResetAllOres()
    {
        foreach (Ore ore in Ore.AllOres)
        {
            if (ore == null)
                continue;

            ore.ResetOre();
        }
    }
}