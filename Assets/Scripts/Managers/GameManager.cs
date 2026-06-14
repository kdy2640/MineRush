using System.Resources;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public OreManager oreManager { get; private set; }
    public SceneController Scene { get; private set; }
    // public UpgradeManager Upgrade { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        oreManager = new OreManager();
        Scene = GetComponent<SceneController>();
        //Upgrade = new UpgradeManager();
    }
}