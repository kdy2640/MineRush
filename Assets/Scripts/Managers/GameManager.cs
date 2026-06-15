using System.Resources;
using UnityEngine;


// 전역으로 접근 가능한 게임매니저.
// Start에서 GameManager.Instance를 통해 접근해주세요. <- Awake에서 초기화하기 때문에 보수적인 체킹
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public OreManager OreManager { get; private set; }
    public SceneController Scene { get; private set; }
    public UpgradeManager Upgrade { get; private set; }
    public GameLoopManager GameLoop { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        OreManager = GetComponent<OreManager>();
        Scene = GetComponent<SceneController>();
        Upgrade = GetComponent<UpgradeManager>();
        GameLoop = GetComponent<GameLoopManager>();
    }
}