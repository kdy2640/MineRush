using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false); 
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void OnClickYes()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void OnClickNo()
    {
        Hide();
    }
}
