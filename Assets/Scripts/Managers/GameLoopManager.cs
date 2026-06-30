using System;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    public bool IsGameLoopScene => GameManager.Instance.Scene.currenSceneType == SceneType.GameLoop; 
    private float loopDuration = 20f;
     
    private GameLoopEventManager eventManager; 
    private SkillEventProxy eventProxy;
    private Action<float> OnTick;
    private float timer;
    private bool isRunning = false;

    public float Timer { get { return timer; } }
    public bool IsRunning => isRunning;
    public GameLoopEventManager Events => eventManager; 
    public SkillEventProxy SkillProxy => eventProxy;


    private void Awake()
    { 
        eventManager = new GameLoopEventManager(); 
        eventProxy = new SkillEventProxy();
    } 
    public void BeforeStart()
    {
    }
    public void StartLoop()
    {
        if (!IsGameLoopScene) return;
        timer = loopDuration + GameManager.Instance.Upgrade.GetRuntimeStat().ExtraDuration;
        OnTick?.Invoke(Timer);
        isRunning = true;

        eventManager.Invoke(GameLoopEventType.LoopStarted);
    }

    private void Update()
    {
        if (!isRunning)
            return;

        timer -= Time.deltaTime; 
        OnTick?.Invoke(Timer);

        if (timer <= 0f)
            EndLoop();
    }

    public void EndLoop()
    {
        if (!isRunning) return;
        if (!IsGameLoopScene) return;
        isRunning = false;
        eventManager.Invoke(GameLoopEventType.LoopEnded);
    }

    public void SubscribeTick(Action<float> ev)
    {
        OnTick += ev;
    }
    public void UnSubscribeTick(Action<float> ev)
    {
        OnTick -= ev;
    }
    public void Restart()
    {
        GameManager.Instance.Scene.RestartScene(SceneType.GameLoop);
    }

}