using UnityEngine;

public class GameLoopUIRoot : MonoBehaviour
{
    [SerializeField] private StartUI startUI;

    [SerializeField] private TimerUI timerUI;

    [SerializeField] private XPBarUI xpBarUI;

    [SerializeField] private EndUI endUI;

    [Header("Timer")]
    [SerializeField] private float startTime;
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
}
