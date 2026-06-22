using UnityEngine;

public class StatButton : MonoBehaviour
{
    [SerializeField] private StatPanelUI statPanel;

    public void OnClick()
    {
        statPanel.Toggle();
    }
}