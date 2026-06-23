using UnityEngine;
using UnityEngine.UI;

public class UI_TabChangeButtonInUpgradeScene : MonoBehaviour
{
    
    [SerializeField] private UI_UpgradeUIController upgradeUIController;
    [SerializeField] private UpgradePanelType panelType;
    
    private Button button;
    
    private void Awake()
    {
        button = GetComponent<Button>();

        if (upgradeUIController == null)
        {
            upgradeUIController = GetComponentInParent<UI_UpgradeUIController>();
        }
    }
    public void OnClick()
    {
        upgradeUIController.ShowPanel(panelType);
    }
}
