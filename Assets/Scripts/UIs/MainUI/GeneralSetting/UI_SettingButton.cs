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
        if(isOpen)
        {
            settingInstance.gameObject.SetActive(false);
            settingInstance.Close();
            isOpen = false;
        }
        else
        {
            settingInstance.gameObject.SetActive(true);
            settingInstance.Open();
            isOpen = true;
        }
    }
}
