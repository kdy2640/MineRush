using UnityEngine;
using System.Collections;

public class GameFlowController : MonoBehaviour
{
    [SerializeField] private StartUI startUI;
    [SerializeField] private MiningSystem miningSystem;
    [SerializeField] private TimerSystem timerSystem;
    [SerializeField] private ResultUI resultUI;
    [SerializeField] private OreResetSystem oreResetSystem;

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
        oreResetSystem.ResetAllOres();

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

        StartCoroutine(EndRoutine());
    }

    private IEnumerator EndRoutine()
    {
        GameLoopEvents.OnGameEnded?.Invoke();

        yield return new WaitForSeconds(1.5f);

        resultUI.Show();
    }
}