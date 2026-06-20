using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeNodePanelController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler
{
    [Header("노드 설명창")]
    [SerializeField] private RectTransform nodeInfoPanelRect;
    [SerializeField] private Vector2 nodeInfoPanelOffset;
    private UI_NodeInfoPanel nodeInfoPanel;

    [Header("노드 연결선 관련")]
    [SerializeField] private RectTransform nodeLinesRect;
    [SerializeField] private GameObject nodeLinePrefab;
    [SerializeField] private float nodeLineThickness = 8f;
    
    [Header("노드 배치 로직관련")]
    [SerializeField] private RectTransform nodesRect;
    [SerializeField] private UpgradeNode rootNode;
    
    [Header("줌 및 이동")]
    [SerializeField] private RectTransform panZoomTargetRect;
    [SerializeField] private float minZoom = 0.5f;
    [SerializeField] private float maxZoom = 1.5f;
    [SerializeField] private float zoomSpeed = 0.1f;
    [SerializeField] private Vector2 minPanPosition = new Vector2(-1000f, -1000f);
    [SerializeField] private Vector2 maxPanPosition = new Vector2(1000f, 1000f);
    
    private RectTransform panZoomParentRect;
    private Vector2 dragStartPointerPosition;
    private Vector2 dragStartPanelPosition;
    private bool isDragging;
    
    private UpgradeNode currentDescriptionNode; // 드래그등의 실시간 위치 갱신할때 필요한 현재 노드 변수
    
    // todo 남은건 설명창에 이름, 설명, 재료(이미지포함?) 갱신
    // todo 저장 시스템을 가정한 node 불러오기 기능도 구현은 해놓음.

    private void Awake()
    {
        nodeInfoPanel = nodeInfoPanelRect.GetComponent<UI_NodeInfoPanel>();
        panZoomParentRect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        InitUpgradeNodePanel();
    }

    private void InitUpgradeNodePanel()
    {
        ShowDescriptionPanel(false);
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

    public void ShowDescriptionPanel(bool active, UpgradeNode upgradeNode = null)
    {
        if (!active)
        {
            nodeInfoPanelRect.gameObject.SetActive(false);
            return;
        }

        currentDescriptionNode = upgradeNode;
        RefreshDescriptionPanelPosition();
        nodeInfoPanelRect.SetAsLastSibling(); // 노드 뒤에 위치하는 상황 방지용 맨위에 띄우는 내장 함수.

        RefreshDescriptionPanel(upgradeNode.upgradeData);

        nodeInfoPanelRect.gameObject.SetActive(true);
    } // 노드에 마우스를 올렸을 때 설명창을 켜고, 마우스가 빠졌을 때 설명창을 끄는 함수.
    // 설명창이 켜질 때 현재 설명 대상 노드를 저장해두고,
    // 이후 드래그나 줌이 발생하면 이 노드를 기준으로 설명창 위치를 다시 계산.
    // 일단 setactive 방식으로 구현함.
    // 하지만 fade 애니메이션 같은 것을 넣고싶으면 캔버스 그룹 방식으로 전환해야 할 수 있음.

    public void RefreshDescriptionPanel(UpgradeData upgradeData)
    {
        nodeInfoPanel.SetInfo(GameManager.Instance.Upgrade.GetState(upgradeData));
    }

    private void RefreshDescriptionPanelPosition()
    {
        if (currentDescriptionNode == null)
        {
            return;
        }

        nodeInfoPanelRect.anchoredPosition =
            GetNodeScreenAnchoredPosition(currentDescriptionNode) + GetScaledDescriptionOffset();
    } // 현재 설명 대상 노드의 화면 위치를 기준으로 설명창 위치를 다시 맞추는 함수.
    // 매 프레임 호출하지 않고, 설명창이 켜질 때 / 드래그할 때 / 줌할 때만 호출한다.

    private Vector2 GetNodeScreenAnchoredPosition(UpgradeNode upgradeNode)
    {
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(null, upgradeNode.rectTransform.position);
        RectTransform descriptionParentRect = nodeInfoPanelRect.parent as RectTransform;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            descriptionParentRect,
            screenPosition,
            null,
            out Vector2 anchoredPosition
        );

        return anchoredPosition;
    } // NodeContent 안에서 이동/확대된 노드의 실제 화면 위치를 가져와서,
    // DescriptionPanel 부모 기준 anchoredPosition으로 변환하는 함수.
    // DescriptionPanel이 NodeContent 밖에 있어서 좌표계를 맞추기 위해 사용한다.

    private Vector2 GetScaledDescriptionOffset()
    {
        if (panZoomTargetRect == null)
        {
            return nodeInfoPanelOffset;
        }

        float currentZoom = panZoomTargetRect.localScale.x;

        return nodeInfoPanelOffset * currentZoom;
    } // 현재 줌 스케일에 맞춰 설명창 offset을 보정하는 함수.
    // 예를 들어 descriptionOffset.y가 200이고 줌 스케일이 0.4라면,
    // 실제 y offset은 80이 되어 노드와 설명창 사이 간격도 줌 비율에 맞게 줄어든다.

    private void ClearNodeLines()
    {
        for (int i = nodeLinesRect.childCount - 1; i >= 0; i--)
        {
            Destroy(nodeLinesRect.GetChild(i).gameObject);
        }
    }

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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        if (panZoomTargetRect == null || panZoomParentRect == null)
        {
            return;
        }

        isDragging = true;
        dragStartPanelPosition = panZoomTargetRect.anchoredPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            panZoomParentRect,
            eventData.position,
            eventData.pressEventCamera,
            out dragStartPointerPosition
        );
    } // 좌클릭 드래그가 시작될 때, 처음 마우스 위치와 패널 위치를 저장하는 함수.
    // 이후 드래그 중 이동량을 계산하기 위한 기준점으로 사용한다.

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging)
        {
            return;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            panZoomParentRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 currentPointerPosition
        );

        Vector2 dragDelta = currentPointerPosition - dragStartPointerPosition;
        Vector2 targetPosition = dragStartPanelPosition + dragDelta;

        SetPanPosition(targetPosition);
        RefreshDescriptionPanelPosition();
    } // 드래그 중인 마우스 위치를 기준으로 패널 위치를 이동시키는 함수.
    // 처음 눌렀던 위치와 현재 위치의 차이만큼 RectTransform의 anchoredPosition을 바꾼다.
    // 설명창이 켜져 있는 경우 설명창 위치도 같이 다시 갱신.

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    } // 드래그가 끝났을 때 드래그 상태를 해제하는 함수.

    public void OnScroll(PointerEventData eventData)
    {
        if (panZoomTargetRect == null || panZoomParentRect == null)
        {
            return;
        }

        float currentZoom = panZoomTargetRect.localScale.x;
        float targetZoom = currentZoom + eventData.scrollDelta.y * zoomSpeed;
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);

        if (Mathf.Approximately(currentZoom, targetZoom))
        {
            return;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            panZoomParentRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 mouseLocalPosition
        );

        Vector2 mouseOffsetFromContent = mouseLocalPosition - panZoomTargetRect.anchoredPosition;
        float zoomRatio = targetZoom / currentZoom;
        Vector2 targetPosition = mouseLocalPosition - mouseOffsetFromContent * zoomRatio;

        panZoomTargetRect.localScale = new Vector3(targetZoom, targetZoom, 1f);

        SetPanPosition(targetPosition);
        RefreshDescriptionPanelPosition();
    } // 마우스 위치를 기준으로 줌인/줌아웃 하는 함수.
    // 줌 전 마우스가 가리키던 NodeContent 위치를 기준으로 anchoredPosition을 보정한다.
    // 마지막에 SetPanPosition을 호출해서 이동 제한 범위를 넘지 않게 다시 보정한다.
    
    private void SetPanPosition(Vector2 targetPosition)
    {
        float currentZoom = panZoomTargetRect.localScale.x;

        Vector2 scaledMinPanPosition = minPanPosition * currentZoom;
        Vector2 scaledMaxPanPosition = maxPanPosition * currentZoom;

        float clampedX = Mathf.Clamp(targetPosition.x, scaledMinPanPosition.x, scaledMaxPanPosition.x);
        float clampedY = Mathf.Clamp(targetPosition.y, scaledMinPanPosition.y, scaledMaxPanPosition.y);

        panZoomTargetRect.anchoredPosition = new Vector2(clampedX, clampedY);
    } // 패널 위치를 실제로 적용하는 함수.
    // 이동 제한 범위도 현재 줌 스케일에 맞춰 같이 줄이거나 늘린다.
    // 마우스 기준 줌으로 계산된 위치가 범위를 넘으면 여기서 다시 제한한다.
}