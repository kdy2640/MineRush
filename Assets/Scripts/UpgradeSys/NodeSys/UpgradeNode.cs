using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Events;
#endif

public class UpgradeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Node Data")]
    [SerializeField] private string nodeId; // 인스펙터에서 설정안함.
    [field: SerializeField] public UpgradeData upgradeData { get; private set; }
    [field: SerializeField] public List<UpgradeNode> connectedNodes { get; private set; } = new();
    [field: SerializeField] public bool isUnlocked { get; private set; }

    private UpgradeState upgradeState;

    [Header("UI")]
    private Button button;
    [SerializeField] private Image upgradeIcon;
    [FormerlySerializedAs("bgImage")] [SerializeField] private Image borderImg;
    private Color maxLevelColor = new Color32(0, 255, 0, 255);

    private UpgradeNodePanelController nodePanelController;
    public RectTransform rectTransform { get; private set; }

    private void Awake()
    {
        nodeId = upgradeData != null ? upgradeData.id : string.Empty;
        isUnlocked = false;
        button = GetComponent<Button>();
        nodePanelController = GetComponentInParent<UpgradeNodePanelController>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void Unlock()
    {
        if (isUnlocked)
        {
            return;
        }

        isUnlocked = true;
        gameObject.SetActive(true);

        upgradeState = GameManager.Instance.Upgrade.GetState(upgradeData);

        if (upgradeData.skill != null)
        {
            GameManager.Instance.Upgrade.RegisterSkillWhenUnlockNode(upgradeState);
        }

        RefreshNodeVisual();
    } // 노드 해금이 곧 0레벨로 state 등록하는 것.

    public void RestoreUnlocked()
    {
        isUnlocked = true;
        gameObject.SetActive(true);
        upgradeState = GameManager.Instance.Upgrade.GetState(upgradeData);
        RefreshNodeVisual();
    } // 저장 / 씬 재진입 시 이미 UpgradeState가 존재하는 노드를 UI에 다시 반영할 때 사용하는 함수.

    public void RefreshNodeVisual()
    {
        if (borderImg == null)
        {
            return;
        }

        if (upgradeData == null)
        {
            borderImg.color = Color.white;
            return;
        }

        UpgradeState state = GameManager.Instance.Upgrade.GetState(upgradeData);

        if (GameManager.Instance.Upgrade.IsMaxLevel(state))
        {
            borderImg.color = maxLevelColor;
            return;
        }

        borderImg.color = Color.white;
    }

    public void TryBuy()
    {
        if (!isUnlocked)
        {
            return;
        }

        if (GameManager.Instance.Upgrade.TryBuyUpgrade(upgradeData))
        {
            nodePanelController.RefreshDescriptionPanel(upgradeData);
            RefreshNodeVisual();

            UpgradeState state = GameManager.Instance.Upgrade.GetState(upgradeData);

            if (state.level == 1)
            {
                foreach (UpgradeNode connectedNode in connectedNodes)
                {
                    if (connectedNode.isUnlocked)
                    {
                        continue;
                    }

                    connectedNode.Unlock();
                    nodePanelController.ConnectNodeLine(this, connectedNode);
                }
            }
        }
    } // 버튼 OnClick에서 직접 호출되는 구매 함수.

    public void OnPointerEnter(PointerEventData eventData)
    {
        nodePanelController.ShowDescriptionPanel(true, this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        nodePanelController.ShowDescriptionPanel(false);
    }

    private void OnDrawGizmos()
    {
        if (connectedNodes == null)
        {
            return;
        }

        Gizmos.color = Color.yellow;

        foreach (UpgradeNode connectedNode in connectedNodes)
        {
            if (connectedNode == null)
            {
                continue;
            }

            DrawGizmoRectLine(transform.position, connectedNode.transform.position, 8f);
        }
    }

    private void DrawGizmoRectLine(Vector3 start, Vector3 end, float thickness)
    {
        Vector3 direction = end - start;
        float length = direction.magnitude;

        if (length <= 0f)
        {
            return;
        }

        Vector3 center = (start + end) * 0.5f;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.right, direction.normalized);

        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(center, rotation, Vector3.one);

        Gizmos.DrawCube(Vector3.zero, new Vector3(length, thickness, 1f));

        Gizmos.matrix = oldMatrix;
    }

    [SerializeField, HideInInspector] private List<UpgradeNode> previousConnectedNodes = new();

    private void OnValidate()
    {
        if (connectedNodes == null)
        {
            connectedNodes = new List<UpgradeNode>();
        }

        RemoveInvalidConnectedNodes();
        RemoveDuplicateConnectedNodes();
        RemoveRemovedBidirectionalConnections();
        SyncBidirectionalConnections();
        CacheConnectedNodes();
    }

    [ContextMenu("Refresh Node By UpgradeData")]
    private void RefreshNodeByUpgradeData()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }

        if (upgradeData == null)
        {
            nodeId = string.Empty;
            gameObject.name = GetDefaultNodeName();

            if (upgradeIcon != null)
            {
                upgradeIcon.sprite = null;
            }

#if UNITY_EDITOR
            RefreshButtonEvent();
#endif

            return;
        }

        nodeId = upgradeData.id;
        gameObject.name = GetUpgradeNodeName();

        if (upgradeIcon != null)
        {
            upgradeIcon.sprite = upgradeData.displayIcon;
        }

#if UNITY_EDITOR
        RefreshButtonEvent();
#endif
    } // UpgradeData 기준으로 노드 이름, 아이콘, 버튼 OnClick 이벤트를 갱신하는 함수.

#if UNITY_EDITOR
    private void RefreshButtonEvent()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }

        if (button == null)
        {
            return;
        }

        if (HasPersistentTryBuyEvent())
        {
            return;
        }

        UnityEventTools.AddPersistentListener(button.onClick, TryBuy);

        EditorUtility.SetDirty(button);
        EditorUtility.SetDirty(this);
    } // 에디터에서 Button OnClick에 TryBuy 함수를 인스펙터 이벤트로 등록하는 함수.

    private bool HasPersistentTryBuyEvent()
    {
        int eventCount = button.onClick.GetPersistentEventCount();

        for (int i = 0; i < eventCount; i++)
        {
            if (button.onClick.GetPersistentTarget(i) == this &&
                button.onClick.GetPersistentMethodName(i) == nameof(TryBuy))
            {
                return true;
            }
        }

        return false;
    } // Button OnClick에 이미 TryBuy가 등록되어 있는지 확인해서 중복 등록을 막는 함수.
#endif

    private string GetDefaultNodeName()
    {
        int siblingIndex = transform.GetSiblingIndex();
        return $"Node{siblingIndex + 1}";
    } // UpgradeData가 없을 때 사용하는 기본 노드 이름을 반환하는 함수.

    private string GetUpgradeNodeName()
    {
        if (!string.IsNullOrEmpty(upgradeData.displayName))
        {
            return upgradeData.displayName;
        }

        return upgradeData.name;
    } // UpgradeData가 있을 때 사용할 노드 이름을 반환하는 함수.

    private void RemoveInvalidConnectedNodes()
    {
        for (int i = connectedNodes.Count - 1; i >= 0; i--)
        {
            if (connectedNodes[i] == this)
            {
                connectedNodes.RemoveAt(i);
            }
        }
    }

    private void RemoveDuplicateConnectedNodes()
    {
        for (int i = connectedNodes.Count - 1; i >= 0; i--)
        {
            UpgradeNode node = connectedNodes[i];

            if (node == null)
            {
                continue;
            }

            for (int j = i - 1; j >= 0; j--)
            {
                if (connectedNodes[j] == node)
                {
                    connectedNodes.RemoveAt(i);
                    break;
                }
            }
        }
    }

    private void RemoveRemovedBidirectionalConnections()
    {
        foreach (UpgradeNode previousNode in previousConnectedNodes)
        {
            if (previousNode == null)
            {
                continue;
            }

            if (connectedNodes.Contains(previousNode))
            {
                continue;
            }

            if (previousNode.connectedNodes.Contains(this))
            {
                previousNode.connectedNodes.Remove(this);

#if UNITY_EDITOR
                EditorUtility.SetDirty(previousNode);
#endif
            }
        }
    }

    private void SyncBidirectionalConnections()
    {
        foreach (UpgradeNode connectedNode in connectedNodes)
        {
            if (connectedNode == null)
            {
                continue;
            }

            if (!connectedNode.connectedNodes.Contains(this))
            {
                connectedNode.connectedNodes.Add(this);

#if UNITY_EDITOR
                EditorUtility.SetDirty(connectedNode);
#endif
            }
        }
    }

    private void CacheConnectedNodes()
    {
        previousConnectedNodes.Clear();

        foreach (UpgradeNode connectedNode in connectedNodes)
        {
            if (connectedNode == null)
            {
                continue;
            }

            previousConnectedNodes.Add(connectedNode);
        }

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
}