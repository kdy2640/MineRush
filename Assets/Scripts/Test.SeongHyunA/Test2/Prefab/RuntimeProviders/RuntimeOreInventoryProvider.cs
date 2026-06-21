using UnityEngine;

public class RuntimeOreInventoryProvider : MonoBehaviour, IOreInventoryProvider
{
    [SerializeField] private OreManager oreManager;

    private void Awake()
    {
        if (oreManager == null)
        {
            oreManager = FindFirstObjectByType<OreManager>();
        }
    }

    public int GetAmount( OreType type)
    {
        if (oreManager == null) return 0;

        return oreManager.GetAmount(type);
    }
}