using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RestartButton : MonoBehaviour
{
    [SerializeField]private GameStateController gameStateController;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        if(gameStateController == null)
        {
            gameStateController = Object.FindFirstObjectByType<GameStateController>();
        }

        if(button != null)
        {
            button.onClick.AddListener(OnClicked);
        }
    }
    private void OnClicked()
    {
        if(gameStateController != null)
        {
            gameStateController.RestartGame();
        }
        else
        {
            Debug.Log("씬에서 GamestateController를 찾을 수 없습니다.");
        }
    }
}
