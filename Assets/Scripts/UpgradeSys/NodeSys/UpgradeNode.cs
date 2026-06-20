using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Node Data")]
    [SerializeField] private string nodeId; //인스펙터에서 설정안함.
    [field: SerializeField] public UpgradeData upgradeData { get; private set; }
    [field: SerializeField] public List<UpgradeNode> connectedNodes { get; private set; } = new();
    [field: SerializeField] public bool isUnlocked { get; private set; }
    
    [Header("UI")]
    private Button button;
    [SerializeField] private Image upgradeIcon;
    
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

    private void OnEnable()
    {
        button.onClick.AddListener(TryBuy);
    }

    public void Unlock()
    {
        if (isUnlocked)
        {
            return;
        }

        isUnlocked = true;
        gameObject.SetActive(true);

        UpgradeState state = GameManager.Instance.Upgrade.GetState(upgradeData);

        if (upgradeData.skill != null)
        {
            GameManager.Instance.Upgrade.RegisterSkillWhenUnlockNode(state);
        }
    } // 노드 해금이 곧 0레벨로 state 등록 하는것.

    public void RestoreUnlocked()
    {
        isUnlocked = true;
        gameObject.SetActive(true);
    }// 저장 / 씬 재진입처럼 이미 UpgradeState가 존재하는 노드를 UI에 다시 반영할 때 사용하는 함수.
    // GetState나 스킬 등록은 다시 하지 않고, 노드가 열려있다는 화면 상태만 복구한다.
    
    private void TryBuy()
    {
        if (!isUnlocked)
        {
            return;
        } // 잠겨있으면 보통 unactive되있지만 혹시모르니 방지

        if (GameManager.Instance.Upgrade.TryBuyUpgrade(upgradeData))
        {
            nodePanelController.RefreshDescriptionPanel(upgradeData);
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
            } // 0레벨에서 업그레이드해서 1레벨 됐을 때 주변 노드 언락
            
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        nodePanelController.ShowDescriptionPanel(true, this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        nodePanelController.ShowDescriptionPanel(false);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(TryBuy);
    }
    
    // [ContextMenu("Test Buy Node")]
    // private void TestBuyNode()
    // {
    //     foreach (UpgradeNode connectedNode in connectedNodes)
    //     {
    //         if (connectedNode == null)
    //         {
    //             continue;
    //         }
    //
    //         if (connectedNode.isUnlocked)
    //         {
    //             continue;
    //         }
    //         nodePanelController.ConnectNodeLine(this, connectedNode);
    //     }
    // } // 테스트 하고 싶으면 UpgradeNodePanelController의 InitUpgradeNodePanel이 호출되지 않게 하고 테스트하기

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
        if (upgradeData == null)
        {
            nodeId = string.Empty;
            gameObject.name = GetDefaultNodeName();

            if (upgradeIcon != null)
            {
                upgradeIcon.sprite = null;
            }

            return;
        }

        nodeId = upgradeData.id;
        gameObject.name = GetUpgradeNodeName();

        if (upgradeIcon != null)
        {
            upgradeIcon.sprite = upgradeData.displayIcon;
        }
    } // UpgradeData가 들어오거나 빠졌을 때 노드 이름과 아이콘을 갱신하는 함수.
    // 데이터가 있으면 displayName으로 오브젝트 이름을 바꾸고 displayIcon을 적용한다.
    // 데이터가 없으면 부모 기준 자식 번호로 기본 이름을 만들고 아이콘을 비운다.

    private string GetDefaultNodeName()
    {
        int siblingIndex = transform.GetSiblingIndex();
        return $"Node{siblingIndex + 1}";
    } // UpgradeData가 없을 때 사용할 기본 노드 이름을 반환하는 함수.
    // 바로 위 부모 기준 자신의 자식 번호를 사용해서 Node0, Node1 같은 이름을 만든다.

    private string GetUpgradeNodeName()
    {
        if (!string.IsNullOrEmpty(upgradeData.displayName))
        {
            return upgradeData.displayName;
        }

        return upgradeData.name;
    } // UpgradeData가 있을 때 사용할 노드 이름을 반환하는 함수.
    // displayName이 있으면 displayName을 쓰고, 비어있으면 SO 에셋 이름을 대신 사용한다.

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
                UnityEditor.EditorUtility.SetDirty(previousNode);
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
                UnityEditor.EditorUtility.SetDirty(connectedNode);
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
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}