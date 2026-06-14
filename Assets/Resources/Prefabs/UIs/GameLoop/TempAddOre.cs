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
        manager.oreManager.Add(OreType.Cooper, 1);
        manager.oreManager.Add(OreType.Iron, 2);
    }

}
