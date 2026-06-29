using UnityEngine;
using UnityEngine.UI;

public class SessionEndButton : MonoBehaviour
{
    [SerializeField] Button button;
    void Start()
    {
        button.onClick.AddListener(OnButtonClick);
    }
    private void OnDestroy()
    { 
        button.onClick.RemoveListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        GameManager.Instance.GameLoop.EndLoop();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
