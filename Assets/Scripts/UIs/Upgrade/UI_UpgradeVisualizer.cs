using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradeVisualizer : MonoBehaviour
{
    [SerializeField] UpgradeData upgradeData;
    private Button button;
    private GameManager manager;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        manager.Upgrade.TryBuyUpgrade(upgradeData);
    }
    void Start()
    {
        manager = GameManager.Instance;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
