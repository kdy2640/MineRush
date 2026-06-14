using UnityEngine;

public class UI_MainUI : MonoBehaviour
{
    [SerializeField] UI_OrePanel orePanel;
    GameManager manager;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject); 
    }
    void Start()
    {
        manager = GameManager.Instance;
        manager.oreManager.SubscribeOreChange(UpdateOre);
        UpdateOre();
    }
    private void OnDestroy()
    {
        manager.oreManager.UnSubscribeOreChange(UpdateOre);
    }
    public void UpdateOre()
    {
        orePanel.RefreshUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
