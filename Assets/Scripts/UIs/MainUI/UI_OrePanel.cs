using System.Collections.Generic;
using UnityEngine;

public class UI_OrePanel : MonoBehaviour
{
    [SerializeField] private GameObject UI_OreAmountPrefab;

    private GameManager manager;
    private readonly List<UI_OreAmountVisualizer> oreUIs = new();

    private void Start()
    {
        manager = GameManager.Instance;

        CacheExistingOreUIs();

        manager.OreManager.SubscribeOreChange(OnOreChangeHandler);
        OnOreChangeHandler();
    }

    private void OnDestroy()
    {
        if (manager != null)
            manager.OreManager.UnSubscribeOreChange(OnOreChangeHandler);
    }

    private void CacheExistingOreUIs()
    {
        oreUIs.Clear();

        UI_OreAmountVisualizer[] existingVisualizers =
            GetComponentsInChildren<UI_OreAmountVisualizer>(true);

        foreach (UI_OreAmountVisualizer visualizer in existingVisualizers)
        {
            oreUIs.Add(visualizer);
        }
    }

    public void OnOreChangeHandler()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        int oreTypeCount = (int)OreType.Length;

        while (oreUIs.Count < oreTypeCount)
        {
            GameObject go = Instantiate(UI_OreAmountPrefab, transform);
            UI_OreAmountVisualizer visualizer = go.GetComponent<UI_OreAmountVisualizer>();

            oreUIs.Add(visualizer);
        }

        for (int i = 0; i < oreTypeCount; i++)
        {
            OreType oreType = (OreType)i;
            int nowAmount = manager.OreManager.GetAmount(oreType);

            oreUIs[i].gameObject.SetActive(true);
            oreUIs[i].SetOre(new OreAmount(oreType, nowAmount));
        }

        for (int i = oreTypeCount; i < oreUIs.Count; i++)
        {
            oreUIs[i].gameObject.SetActive(false);
        }
    }
}