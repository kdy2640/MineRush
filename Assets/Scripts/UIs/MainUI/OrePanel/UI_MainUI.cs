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
        manager.OreManager.SubscribeOreChange(UpdateOre);
        UpdateOre();
    }
    private void OnDestroy()
    {
        manager.OreManager.UnSubscribeOreChange(UpdateOre);
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
