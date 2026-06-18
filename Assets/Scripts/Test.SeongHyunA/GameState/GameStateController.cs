using Unity.VisualScripting;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    [Header("GameState UI")]
    [SerializeField] private GameObject startUI;
    [SerializeField] private GameObject resultUI;
    [SerializeField] private GameObject gameOverUI;

    [Header("References for Restart")]
    [SerializeField] private StageTimerPanel timerPanel;
    [SerializeField] private Transform oreContainer;
    [SerializeField] private StageRewardPanel rewardPanel;

    private LocalGameState state;

    private void Start()
    {
        SetState(LocalGameState.Start);
    }
    public void StartGame()
    {
        SetState(LocalGameState.Playing);
        if (timerPanel != null) timerPanel.StartTimer();
    }
    public void RestartGame()
    {
        SetState(LocalGameState.Playing);

        if (timerPanel != null)
        {
            timerPanel.ResetTimer();
            timerPanel.StartTimer();
        }

        if (oreContainer != null)
        {

            foreach (Transform oreTransform in oreContainer)
            {

                oreTransform.gameObject.SetActive(true);

                oreTransform.localScale = Vector3.one;
            }
        }
    }
    public void ShowResult()
    {
        SetState(LocalGameState.Result);
    }
    public void GameOver()
    {
        SetState(LocalGameState.GameOver);
    }
    private void SetState(LocalGameState newState)
    {
        state = newState;

        startUI.SetActive(false);
        resultUI.SetActive(false);
        gameOverUI.SetActive(false);

        switch(state)
        {
            case LocalGameState.Start: startUI.SetActive(true); break;

            case LocalGameState.Playing: break;
            
            case LocalGameState.Result: resultUI.SetActive(true); break;

            case LocalGameState.GameOver: gameOverUI.SetActive(true); break;
        }
    }

}
