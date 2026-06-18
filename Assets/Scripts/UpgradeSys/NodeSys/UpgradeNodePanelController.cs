using System;
using UnityEngine;

public class UpgradeNodePanelController : MonoBehaviour
{
    [SerializeField] private RectTransform descriptionPanel;
    [SerializeField] private Vector2 descriptionOffset;

    [SerializeField] private RectTransform nodeLinesRect;
    [SerializeField] private GameObject nodeLinePrefab;
    [SerializeField] private float nodeLineThickness = 8f;
    
    [SerializeField] private RectTransform nodesRect;
    [SerializeField] private UpgradeNode rootNode;
    
    // todo 남은건 설명창에 이름, 설명, 재료(이미지포함?) 갱신
    // todo 저장 시스템을 가정한 node 불러오기 기능도 구현은 해놓음.

    private void Start()
    {
        InitUpgradeNodePanel();
    }

    private void InitUpgradeNodePanel()
    {
        UpgradeNode[] nodes = GetComponentsInChildren<UpgradeNode>(true);

        HideAllNodes(nodes);
        ClearNodeLines();

        if (rootNode == null)
        {
            Debug.LogWarning("Root Node가 설정되지 않았습니다.");
            return;
        }

        RestoreNodesByUpgradeState(nodes);

        if (!GameManager.Instance.Upgrade.HasState(rootNode.upgradeData))
        {
            rootNode.Unlock();
        }

        RefreshUnlockedLines(nodes);
    }// 노드 패널 초기화 함수.
    // 에디터에 배치된 모든 노드를 일단 끄고,
    // UpgradeManager에 저장된 UpgradeState를 기준으로 열려있던 노드만 다시 켠다.
    // 처음 시작이라 rootNode 상태가 없으면 rootNode만 새로 해금한다.
    
    private void HideAllNodes(UpgradeNode[] nodes)
    {
        foreach (UpgradeNode node in nodes)
        {
            node.gameObject.SetActive(false);
        }
    }
    private void ClearNodeLines()
    {
        for (int i = nodeLinesRect.childCount - 1; i >= 0; i--)
        {
            Destroy(nodeLinesRect.GetChild(i).gameObject);
        }
    }
    private void RestoreNodesByUpgradeState(UpgradeNode[] nodes)
    {
        foreach (UpgradeNode node in nodes)
        {
            if (GameManager.Instance.Upgrade.HasState(node.upgradeData))
            {
                node.RestoreUnlocked();
            }
        }
    }// UpgradeManager에 State가 등록된 노드만 다시 켜는 함수.
    // State가 있다는 건 해당 노드가 이미 해금된 적 있다는 뜻으로 본다.

    private void RefreshUnlockedLines(UpgradeNode[] nodes)
    {
        foreach (UpgradeNode node in nodes)
        {
            if (!node.isUnlocked)
            {
                continue;
            }

            foreach (UpgradeNode connectedNode in node.connectedNodes)
            {
                if (connectedNode == null)
                {
                    continue;
                }

                if (!connectedNode.isUnlocked)
                {
                    continue;
                }

                if (node.GetInstanceID() > connectedNode.GetInstanceID())
                {
                    continue;
                } // 오브젝트의 유니티의 고유id를 비교해서 양방향 연결 상황일때, 한쪽에서 선만 그림.

                ConnectNodeLine(node, connectedNode);
            }
        }
    }// 현재 열려있는 노드들끼리 연결선을 다시 그리는 함수.
    // connectedNodes가 양방향이라 같은 선이 두 번 생길 수 있으므로
    // GetInstanceID를 비교해서 한쪽에서만 선을 생성한다.

    public void ShowDescriptionPanel(bool active, UpgradeNode upgradeNode = null)
    {
        if (!active)
        {
            descriptionPanel.gameObject.SetActive(false);
            return;
        }

        descriptionPanel.anchoredPosition = upgradeNode.rectTransform.anchoredPosition + descriptionOffset;
        descriptionPanel.SetAsLastSibling(); // 노드 뒤에 위치하는 상황 방지용 맨위에 띄우는 내장 함수.

        RefreshDescriptionPanel(upgradeNode.upgradeData);

        descriptionPanel.gameObject.SetActive(true);
    } // 일단 setactive 방식으로 구현함.
    // 하지만 fade 애니메이션 같은 것을 넣고싶으면 캔버스 그룹 방식으로 전환해야 함.

    private void RefreshDescriptionPanel(UpgradeData upgradeData)
    {
        // TODO:
        // 노드 이름 갱신
        // 설명 갱신
        // 필요한 자원량 갱신
    }

    public void ConnectNodeLine(UpgradeNode currentNode, UpgradeNode targetNode)
    {
        if (currentNode == null || targetNode == null)
        {
            return;
        }

        GameObject lineObject = Instantiate(nodeLinePrefab, nodeLinesRect);
        RectTransform lineRect = lineObject.GetComponent<RectTransform>();

        Vector2 startPos = currentNode.rectTransform.anchoredPosition;
        Vector2 endPos = targetNode.rectTransform.anchoredPosition;

        Vector2 direction = endPos - startPos;
        float distance = direction.magnitude;

        lineRect.anchoredPosition = startPos + direction * 0.5f;
        lineRect.sizeDelta = new Vector2(distance, nodeLineThickness);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        lineRect.localRotation = Quaternion.Euler(0f, 0f, angle);

        nodeLinesRect.SetAsFirstSibling();
    }
    
}
