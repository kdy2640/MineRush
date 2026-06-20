using UnityEngine;

public class GameFlowController : MonoBehaviour
{
    [SerializeField] private StartUI startUI;
    [SerializeField] private MiningSystem miningSystem;
    [SerializeField] private TimerSystem timerSystem;
    [SerializeField] private ResultUI resultUI;

    private bool running;

    private void Start()
    {
        startUI.OnFinished += StartGame;
        startUI.Play();
    }

    private void StartGame()
    {
        running = true;

        timerSystem.StartTimer();
        miningSystem.StartMining();
    }

    public void RestartGame()
    {
        TestOreManager.Instance.ResetAllOres();

        RewardSystem.Instance.ClearSession();

        XPSystem.Instance.ResetSession();

        resultUI.gameObject.SetActive(false);

        timerSystem.StartTimer();

        miningSystem.StartMining();

        running = true;
    }

    public void EndGame()
    {
        if (!running)
            return;

        running = false;

        miningSystem.StopMining();

        resultUI.Show();
    }
}