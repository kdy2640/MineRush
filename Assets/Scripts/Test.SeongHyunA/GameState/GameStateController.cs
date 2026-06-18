using Unity.VisualScripting;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    [Header("GameState UI")]
    [SerializeField] private GameObject startUI;
    [SerializeField] private GameObject resultUI;
    [SerializeField] private GameObject gameOverUI;

    private LocalGameState state;

    private void Start()
    {
        SetState(LocalGameState.Start);
    }
    public void StartGame()
    {
        SetState(LocalGameState.Playing);
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
            
            case LocalGameState.Result: resultUI.SetActive(true); break;

            case LocalGameState.GameOver: gameOverUI.SetActive(true); break;
        }
    }

}
