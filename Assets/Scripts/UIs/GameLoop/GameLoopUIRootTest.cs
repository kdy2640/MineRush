using System.Collections.Generic;
using UnityEngine;

public class GameLoopUIRoot : MonoBehaviour
{
    [SerializeField] private StartUI startUI;

    [SerializeField] private TimerUI timerUI;

    [SerializeField] private XPBarUI xpBarUI;

    [SerializeField] private EndUI endUI;

    [Header("Timer")]
    [SerializeField] private float startTime = 10f;

    [Header("UI Test")]
    [SerializeField] private ResultUI resultUI;

    [SerializeField] private RuntimeStatPanel runtimeStatPanel;

    public StartUI StartUI => startUI;
    public TimerUI TimerUI => timerUI;
    public XPBarUI XPBarUI => xpBarUI;
    public EndUI EndUI => endUI;

    private float currentTime;
    private bool timerRunning;

    private void Awake()
    {
        timerUI.OnTimerEnded += HandleTimerEnd;
    }

    private void Start()
    {
        TestResultUI();

        TestRuntimeStatPanel();

        timerUI.SetTime(startTime);

        startUI.onFinished.AddListener(StartTimer);

        startUI.Play();
    }

    private void StartTimer()
    {
        currentTime = startTime;

        timerRunning = true;
    }

    private void HandleTimerEnd()
    {
        timerRunning = false;

        endUI.Play();
    }

    private void Update()
    {
        if (!timerRunning) return;

        currentTime -= Time.deltaTime;

        timerUI.SetTime(currentTime);
    }

    [ContextMenu("Test Result UI")]
    private void TestResultUI()
    {
        List<OreAmount> test = new()
        {
            new OreAmount(OreType.Copper, 10),
            new OreAmount(OreType.Iron, 20),
            new OreAmount(OreType.Gold, 5),
            new OreAmount(OreType.Diamond, 1)
        };

        resultUI.gameObject.SetActive(true);

        resultUI.SetData(test);
    }

    [ContextMenu("Test Runtime Stat Panel")]
    private void TestRuntimeStatPanel()
    {
        RuntimeStat stat = new RuntimeStat();

        runtimeStatPanel.gameObject.SetActive(true);

        runtimeStatPanel.SetData(stat);
    }
}