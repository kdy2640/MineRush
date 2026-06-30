using System.Collections.Generic;
using UnityEngine; 

public class UI_OrePanel : MonoBehaviour
{
    [SerializeField] private GameObject UI_OreAmountPrefab;

    private GameManager manager;
    private readonly List<UI_OreAmountVisualizer> oreUIs = new();
    private Dictionary<OreType, int> DisplayAmount = new();
    private bool isDelayRefresh = false;


    public void SetDelayRefresh(bool flag)
    {
        isDelayRefresh = flag;
    }
    private void Awake()
    {
        for (int i = 0; i < (int)OreType.Length; i++)
        {
            OreType nowType = (OreType)i;
            if (!DisplayAmount.ContainsKey(nowType))
            { 
                DisplayAmount.Add(nowType, 0);
            }
        }
    }
    private void Start()
    {
        manager = GameManager.Instance;

        CacheExistingOreUIs();

        manager.OreManager.SubscribeOreChange(OnOreChangeHandler);
        OnOreChangeHandler();
        SetDelayRefresh(manager.GameLoop.IsGameLoopScene);
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
        if (isDelayRefresh) return;
        RefreshUI();
    }

    public void RefreshOneUI(OreAmount nowAmount, bool forceRealValue)
    {
        if (!isDelayRefresh) return;

        OreType type = nowAmount.oreType;
        int amount = nowAmount.amount;
        int index = (int)type;

        if (index < 0 || index >= (int)OreType.Length)
            return;

        // UI 슬롯이 부족하면 index까지 생성
        while (oreUIs.Count <= index)
        {
            GameObject go = Instantiate(UI_OreAmountPrefab, transform);
            UI_OreAmountVisualizer visualizer = go.GetComponent<UI_OreAmountVisualizer>();

            if (visualizer == null)
            {
                Debug.LogError($"{nameof(UI_OreAmountPrefab)}에 UI_OreAmountVisualizer가 없음");
                Destroy(go);
                return;
            }

            oreUIs.Add(visualizer);
            visualizer.gameObject.SetActive(false);
        }

        int displayAmount;

        if (forceRealValue)
        {
            // 실제 OreManager 값으로 화면 값을 맞춤
            displayAmount = manager.OreManager.GetAmount(type);
        }
        else
        {
            // 화면 표시값만 amount만큼 증가
            displayAmount = DisplayAmount[type] + amount;
        }

        DisplayAmount[type] = displayAmount;

        UI_OreAmountVisualizer targetUI = oreUIs[index];

        targetUI.gameObject.SetActive(true);
        targetUI.SetOre(new OreAmount(type, displayAmount));

        if (amount > 0)
            targetUI.PlayGain();
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
            int nowAmount = GameManager.Instance.OreManager.GetAmount(oreType);

            DisplayAmount[oreType] = nowAmount;
            oreUIs[i].gameObject.SetActive(true);
            oreUIs[i].SetOre(new OreAmount(oreType, nowAmount));
        }

        for (int i = oreTypeCount; i < oreUIs.Count; i++)
        {
            oreUIs[i].gameObject.SetActive(false);
        }
    }
}