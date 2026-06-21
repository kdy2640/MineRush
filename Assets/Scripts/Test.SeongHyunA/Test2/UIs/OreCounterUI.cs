using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OreCounterUI : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private OreType oreType;

    [SerializeField]
    private MonoBehaviour providerObject;

    private IOreInventoryProvider provider;

    private void Awake()
    {
        provider =
            providerObject
            as IOreInventoryProvider;
    }

    private void Update()
    {
        if (provider == null)
            return;

        int amount =
            provider.GetAmount(
                oreType);

        text.text =
            amount.ToString();
    }
}