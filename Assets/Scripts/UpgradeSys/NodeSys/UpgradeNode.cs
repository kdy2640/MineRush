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

    [Header("업그레이드 시도 연출")]
    [SerializeField] private NodeUpgradeEffect upgradeEffect;

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

        bool isUpgraded = GameManager.Instance.Upgrade.TryBuyUpgrade(upgradeData);

        upgradeEffect.DisplayUpgradeEffect(isUpgraded);

        if (isUpgraded)
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

#if UNITY_EDITOR
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

            RefreshButtonEvent();
            return;
        }

        nodeId = upgradeData.id;
        gameObject.name = GetUpgradeNodeName();

        if (upgradeIcon != null)
        {
            upgradeIcon.sprite = upgradeData.displayIcon;
        }

        RefreshButtonEvent();
    } // UpgradeData 기준으로 노드 ID, 오브젝트 이름, 아이콘, 버튼 이벤트를 에디터에서 자동 갱신한다.

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
    } // Button OnClick에 TryBuy가 없으면 에디터 인스펙터 이벤트로 자동 등록한다.

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
    } // Button OnClick에 이미 TryBuy가 등록되어 있는지 검사해서 중복 등록을 막는다.

    private string GetDefaultNodeName()
    {
        int siblingIndex = transform.GetSiblingIndex();
        return $"Node{siblingIndex + 1}";
    } // UpgradeData가 없을 때 형제 순서를 기준으로 기본 노드 이름을 만든다.

    private string GetUpgradeNodeName()
    {
        if (!string.IsNullOrEmpty(upgradeData.displayName))
        {
            return upgradeData.displayName;
        }

        return upgradeData.name;
    } // UpgradeData가 있을 때 displayName을 우선 사용하고, 없으면 SO 이름을 노드 이름으로 사용한다.

    private void RemoveInvalidConnectedNodes()
    {
        for (int i = connectedNodes.Count - 1; i >= 0; i--)
        {
            if (connectedNodes[i] == this)
            {
                connectedNodes.RemoveAt(i);
            }
        }
    } // 자기 자신을 연결 노드 목록에서 제거해서 셀프 연결을 막는다.

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
    } // 같은 노드가 연결 목록에 여러 번 들어간 경우 중복 항목을 제거한다.

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
                EditorUtility.SetDirty(previousNode);
            }
        }
    } // 이전에는 연결되어 있었지만 지금은 제거된 노드에서, 반대편 연결도 같이 제거한다.

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
                EditorUtility.SetDirty(connectedNode);
            }
        }
    } // 현재 노드가 A->B로 연결되면 B->A 연결도 자동으로 맞춰준다.

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

        EditorUtility.SetDirty(this);
    } // 현재 연결 상태를 캐싱해서 다음 에디터 갱신 때 삭제된 연결을 비교할 수 있게 한다.

#endif
}