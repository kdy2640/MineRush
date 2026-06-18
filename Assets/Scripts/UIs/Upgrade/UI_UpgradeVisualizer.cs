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
        //업그레이드 테스트용 임시
        manager.Upgrade.RegisterSkillWhenUnlockNode(manager.Upgrade.GetState(upgradeData));
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
