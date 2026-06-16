using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    private bool IsGameLoopScene => GameManager.Instance.Scene.currenSceneType == SceneType.GameLoop;
    [SerializeField] private StoneSpawner spawnerPrefab; 

    [SerializeField] private float loopDuration = 30f;

    private StoneSpawner spawner;
    private GameLoopEventManager eventManager;
    private float timer;
    private bool isRunning;

    public float Timer { get { return timer; } }
    public bool IsRunning => isRunning;
    public IGameLoopEventSubscribable Events => eventManager;

    private void Awake()
    { 
        eventManager = new GameLoopEventManager();
    } 

    public void StartLoop()
    {
        if (!IsGameLoopScene) return;
        timer = loopDuration;
        isRunning = true;

        spawner = GameObject.Instantiate(spawnerPrefab);
        spawner.transform.position = Vector3.zero;
        spawner.RandomSpawn(10);
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