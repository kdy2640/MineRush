using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum UpgradePanelType
{
    UpgradeNode,
    AutoMining,
    Artifact,
    Forge
}

[Serializable]
public class UpgradePanelEntry
{
    public UpgradePanelType type;
    public GameObject panelObject;
}

public class UI_UpgradeUIController : MonoBehaviour
{
    [SerializeField] private List<UpgradePanelEntry> panels = new();
    [SerializeField] private UpgradePanelType defaultPanel = UpgradePanelType.UpgradeNode;

    private UpgradePanelEntry currentPanel;

    private void Awake()
    {
        HideAllPanels();
        ShowPanel(defaultPanel);
    } // 처음 UI가 켜질 때 모든 패널을 끄고, 기본 패널만 켜는 초기화 함수.

    public void ShowPanel(UpgradePanelType type)
    {
        UpgradePanelEntry targetPanel = GetPanel(type);

        if (targetPanel == null)
        {
            Debug.LogWarning($"패널을 찾을 수 없음: {type}");
            return;
        }

        if (currentPanel != null)
        {
            SetPanelActive(currentPanel, false);
        }

        currentPanel = targetPanel;
        SetPanelActive(currentPanel, true);
    } // 원하는 패널 하나만 켜는 함수.
    // 기존에 켜져 있던 패널은 끄고, 인자로 받은 타입의 패널만 켠다.

    private void HideAllPanels()
    {
        foreach (UpgradePanelEntry panel in panels)
        {
            SetPanelActive(panel, false);
        }
    } // 등록된 모든 패널을 끄는 함수.
    // 시작할 때 여러 패널이 동시에 켜져있는 상황을 방지하기 위해 사용한다.

    private UpgradePanelEntry GetPanel(UpgradePanelType type)
    {
        foreach (UpgradePanelEntry panel in panels)
        {
            if (panel.type == type)
            {
                return panel;
            }
        }

        return null;
    } // 인자로 받은 타입과 같은 패널 데이터를 찾아서 반환하는 함수.
    // panels 리스트에서 해당 타입을 못 찾으면 null을 반환한다.

    private void SetPanelActive(UpgradePanelEntry panel, bool active)
    {
        if (panel == null || panel.panelObject == null)
        {
            return;
        }

        panel.panelObject.SetActive(active);
    } // 실제 패널 오브젝트를 켜고 끄는 함수.
    // panel이나 panelObject가 비어있으면 오류 방지를 위해 그냥 return한다.
}