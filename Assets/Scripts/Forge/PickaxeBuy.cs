using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickaxeBuy : MonoBehaviour
{
    [SerializeField] private PickAxesPanel pickAxesPanel;
    [SerializeField] private InputActionReference rightClickAction; // 인스펙터에서 인풋액션 할당
    [SerializeField] private float holdDuration = 1.7f; // 누르고 있을 시간.

    [Header("ui관련")]
    [SerializeField] private CanvasGroup pickaxeBuyPanelCanvasGroup;
    [SerializeField] private PickaxeBuyDisplay pickaxeBuyDisplay;
    private bool isHolding;
    private bool isUpgradeVisualPlaying; // 업그레이드 연출이 끝나기 전까지 입력을 막는 플래그 변수
    private float holdTimer;


    public void Awake()
    {
        HideBuyPanel();
        HideHoldUI();
    }

    private void OnEnable()
    {
        rightClickAction.action.started += OnRightClickStarted;
        rightClickAction.action.canceled += OnRightClickCanceled;
        rightClickAction.action.Enable();
    }

    private void OnDisable()
    {
        rightClickAction.action.started -= OnRightClickStarted;
        rightClickAction.action.canceled -= OnRightClickCanceled;
        rightClickAction.action.Disable();

        ResetHold();
        isUpgradeVisualPlaying = false;
    }

    private void Update()
    {
        if (isUpgradeVisualPlaying)
            return;
        // 업그레이드 연출 중이면 입력 타이머가 돌아가지 않게 막음.

        if (!isHolding)
            return;
        // 우클릭 홀드 중이 아니면 리턴

        holdTimer += Time.deltaTime;

        RefreshHoldUI();
        // 0~1 비율로 계산해서 ui넘김. fill amount에 활용.

        if (holdTimer < holdDuration)
            return;

        CompleteHold();
        //홀딩이 제대로 됐으면 완료함수 실행 
    }

    private void OnRightClickStarted(InputAction.CallbackContext context)
    {
        if (isUpgradeVisualPlaying)
            return;
        //업그레이드 완료후 연출이 끝날때까지 입력을 막기위함.
        if (!pickAxesPanel.CanBuyCurrentPickaxe())
            return;
        // 재료가 부족하거나 이미 구매한 곡괭이면 우클릭 홀드 UI를 시작하지 않는다.

        isHolding = true;
        holdTimer = 0f; // 타이머 초기화
        
        ShowBuyPanel();
        ShowHoldUI();
        RefreshHoldUI();
    }

    private void OnRightClickCanceled(InputAction.CallbackContext context)
    {
        if (isUpgradeVisualPlaying)
            return;
        // 홀드 완료 후 구매 완료 연출 중이면 우클릭을 떼도 아무 반응하지 않는다.
        
        HideBuyPanel();
        ResetHold();
    }

    private void CompleteHold()
    {
        isHolding = false;
        holdTimer = 0f;
        isUpgradeVisualPlaying = true;

        HideHoldUI();
        UpgradePickaxe();
        PlayUpgradeVisual();
    }

    private void ResetHold()
    {
        isHolding = false;
        holdTimer = 0f;

        HideHoldUI();
        RefreshHoldUI();
    }

    private void UpgradePickaxe()
    {
        // TODO: 곡괭이 업그레이드 로직 호출
        Debug.Log("UpgradePickaxe");
        pickAxesPanel.BuySelectedPickaxe();
    }

    private void PlayUpgradeVisual()
    {
        // TODO: 업그레이드 성공 연출 시작
        pickaxeBuyDisplay.PlayBuyCompleteVisual(pickAxesPanel.GetCurrentPickaxe());
    } // 업그레이드가 완료됐을때 업그레이드 연출함수 호출하는 함수
    // 꼭 그쪽에서 연출이 끝나는 시점에 EndUpgradeVisual() 호출

    public void EndUpgradeVisual()
    {
        isUpgradeVisualPlaying = false;
        HideBuyPanel();
        ResetHold();
    } // 업그레이드 연출이 끝났을 때 다시 입력을 받을 수 있게 한다.

    private void ShowBuyPanel()
    {
        pickaxeBuyPanelCanvasGroup.DOKill();
        pickaxeBuyPanelCanvasGroup.DOFade(1f, 0.3f);
        pickaxeBuyPanelCanvasGroup.blocksRaycasts = true;
        pickaxeBuyPanelCanvasGroup.interactable = true;
    }

    private void HideBuyPanel()
    {
        pickaxeBuyPanelCanvasGroup.DOKill();
        pickaxeBuyPanelCanvasGroup.DOFade(0f, 0.3f);
        pickaxeBuyPanelCanvasGroup.blocksRaycasts = false;
        pickaxeBuyPanelCanvasGroup.interactable = false;
    }
    private void ShowHoldUI()
    {
        // TODO: 우클릭 홀드 시작 UI On
        pickaxeBuyDisplay.ShowHoldingUI(true);
    }

    private void HideHoldUI()
    {
        // TODO: 우클릭 홀드 취소/완료 UI Off
        pickaxeBuyDisplay.ShowHoldingUI(false);
    }

    private void RefreshHoldUI()
    {
        // TODO: progress 0~1 기준으로 게이지 갱신
        pickaxeBuyDisplay.SetHoldingFillAmount(Mathf.Clamp01(holdTimer / holdDuration));
    }
}