using UnityEngine;

public class ExitButton : MonoBehaviour
{
    [SerializeField] private GameOverUI gameOverUI;

    public void OnClickExit()
    {
        gameOverUI.Show();
    }
}
