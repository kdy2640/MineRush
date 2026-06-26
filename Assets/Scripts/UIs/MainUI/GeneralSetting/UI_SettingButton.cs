using UnityEngine;
using UnityEngine.UI;

public class UI_SettingButton : MonoBehaviour
{
    [SerializeField] private Button settingButton;
    [SerializeField] private SettingsPopup settingPrefab;

    private SettingsPopup settingInstance;
    private bool isOpen;
    void Start()
    {
        isOpen = false;
        if (settingInstance == null)
        {
            settingInstance = GameObject.Instantiate(settingPrefab);
            settingInstance.gameObject.SetActive(false);
        }
        settingButton.onClick.AddListener(OnClickButton);
    }
    private void OnDestroy()
    {
        settingButton.onClick.RemoveListener(OnClickButton);
    }

    private void OnClickButton()
    {
        settingInstance.gameObject.SetActive(true);
        settingInstance.Open();
    }
}
