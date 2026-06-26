using UnityEngine;
using UnityEngine.UI;

public class UI_BackMainButton : MonoBehaviour
{
    [SerializeField] Button mainButton;
    void Start()
    {
        mainButton = GetComponent<Button>();
        mainButton.onClick.AddListener(OnClickButton);
    }
    private void OnDestroy()
    { 
        mainButton.onClick.RemoveListener(OnClickButton);
    }
    private void OnClickButton()
    {
        GameManager.Instance.Scene.ChangeScene(SceneType.Main);
    }
}
