using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;

    public void Open()
    {
        panel.SetActive(true);
    }

    public void Close()
    {
        panel.SetActive(false);
    }

    public void OnYes()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnNo()
    {
        Close();
    }
}