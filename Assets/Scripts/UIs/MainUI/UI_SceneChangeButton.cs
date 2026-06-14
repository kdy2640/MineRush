using UnityEngine;
using UnityEngine.UI;

public class UI_SceneChangeButton : MonoBehaviour
{
    [SerializeField] private SceneType nextSceneType;

    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
    }
    public void OnButtonClicked()
    {
        GameManager.Instance.Scene.ChangeScene(nextSceneType);
    }

    void Start()
    {
        
    }
     
    void Update()
    {
        
    }
}
