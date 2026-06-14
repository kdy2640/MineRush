using System.Collections.Generic;
using UnityEngine;

public class UI_OrePanel : MonoBehaviour
{
    [SerializeField] private GameObject UI_OreAmountPrefab;

    private GameManager manager;
    private readonly List<UI_OreAmountVisualizer> oreUIs = new();

    private void Awake()
    {
        manager = GameManager.Instance;
    }

    public void RefreshUI()
    {
        int oreTypeCount = (int)OreType.Length;

        // 부족한 UI는 OreType 개수만큼 생성
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