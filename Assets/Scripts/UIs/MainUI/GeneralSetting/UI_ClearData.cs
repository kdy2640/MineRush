using UnityEngine;
using UnityEngine.UI;

public class UI_ClearData : MonoBehaviour
{
    [SerializeField] Button button;



    void Start()
    {
        button.onClick.AddListener(OnClickButtonHandler);
    }

    private void OnDestroy()
    { 
        button.onClick.RemoveListener(OnClickButtonHandler);
    }
    private void OnClickButtonHandler()
    {
        GameManager.Instance.Save.ResetSave();
        GameManager.Instance.Scene.ChangeScene(SceneType.Main, true);
    }
}
