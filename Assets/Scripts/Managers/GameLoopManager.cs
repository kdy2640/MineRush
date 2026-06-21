using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    private bool IsGameLoopScene => GameManager.Instance.Scene.currenSceneType == SceneType.GameLoop; 
    [SerializeField] private float loopDuration = 20f;
     
    private GameLoopEventManager eventManager; 
    private SkillEventProxy eventProxy;
    private float timer;
    private bool isRunning;

    public float Timer { get { return timer; } }
    public bool IsRunning => isRunning;
    public GameLoopEventManager Events => eventManager; 
    public SkillEventProxy SkillProxy => eventProxy;


    private void Awake()
    { 
        eventManager = new GameLoopEventManager(); 
        eventProxy = new SkillEventProxy();
    } 

    public void StartLoop()
    {
        if (!IsGameLoopScene) return;
        timer = loopDuration + GameManager.Instance.Upgrade.GetRuntimeStat().ExtraDuration;
        isRunning = true;

        eventManager.Invoke(GameLoopEventType.LoopStarted);
    }

    private void Update()
    {
        if (!isRunning)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
            EndLoop();
    }

    private void EndLoop()
    {
        if (!IsGameLoopScene) return;
        isRunning = false;
        GameManager.Instance.Scene.ChangeScene(SceneType.Upgrade);
    }
}