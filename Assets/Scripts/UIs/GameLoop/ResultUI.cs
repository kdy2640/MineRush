using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private ResultOreItem itemPrefab;

    [SerializeField] private Transform oreContainer;

    [SerializeField] private Button upgradeButton;

    [SerializeField] private Button nextSessionButton;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform panel;

    //1차로 수집된 oreItem 수량 저장
    private List<ResultOreItem> oreItems = new List<ResultOreItem>();
    private bool isInitialized;

    private void Awake()
    {
        isInitialized = itemPrefab != null && oreContainer != null;

        if (!isInitialized)
        {
            Debug.LogError($"[{nameof(ResultUI)}] 초기화 실패");
        }
        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(() =>
            {
                PlayButtonAnimation(upgradeButton);

                GameManager.Instance.Scene.ChangeScene(SceneType.Upgrade);
            });
        }

        if (nextSessionButton != null)//다음 세션 시작 기능 추가예정
        {
            nextSessionButton.onClick.AddListener(() =>
            {
                PlayButtonAnimation(nextSessionButton);
            });
        }
    }

    public void SetData(List<OreAmount> oreAmounts)
    {
        if (!isInitialized) return;

        if (oreAmounts == null)
        {
            Debug.LogWarning("[ResultUI] 전달된 데이터가 null입니다.");
            return;
        }

        Clear();

        foreach (OreAmount oreAmount in oreAmounts)
        {
            CreateItem(oreAmount);
        }
        PlayBonusAnimation(oreAmounts);
    }

    private void CreateItem(OreAmount oreAmount)
    {
        if (oreAmount == null) return;

        ResultOreItem oreItem = Instantiate(itemPrefab, oreContainer);

        if (oreItem == null)
        {
            Debug.LogWarning("[ResultUI] 아이템 생성 실패");
            return;
        }

        oreItem.SetData(oreAmount);

        oreItems.Add(oreItem);//수량 5배 변경 전 저장
    }

    private void Clear()
    {
        if (oreContainer == null) return;

        for (int i = oreContainer.childCount - 1; i >= 0; i--)
        {
            Transform child = oreContainer.GetChild(i);

            if (child == null) continue;

            Destroy(child.gameObject);
        }
            oreItems.Clear();//1차 광석수량 데이터(List) 삭제
    }
    public void Show()
    {
        gameObject.SetActive(true);

        canvasGroup.alpha = 0f;

        panel.localScale = Vector3.one * 0.8f;

        Sequence seq = DOTween.Sequence();

        seq.Join(canvasGroup.DOFade(1f, 0.4f));

        seq.Join(panel.DOScale(1f, 0.45f).SetEase(Ease.OutBack));
    }

    public void Hide()
    {
        Sequence seq = DOTween.Sequence();

        seq.Join(canvasGroup.DOFade(0f, 0.25f));

        seq.Join(panel.DOScale(0.85f, 0.25f));

        seq.OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
   
    private void PlayButtonAnimation(Button button)
    {
        if(button == null) return;

        RectTransform rect = button.GetComponent<RectTransform>();

        rect.DOKill();

        Sequence seq = DOTween.Sequence();

        seq.Append(rect.DOScale(0.9f, 0.09f));

        seq.Append(rect.DOScale(1.0f, 0.12f));
    }
    private void PlayBonusAnimation(List<OreAmount> oreAmounts)
    {
        // 첫 번째 변경 전까지 대기 시간
        float startDelay = 0.6f;

        // 각 아이템 사이의 변경 간격
        float interval = 0.15f;

        for (int i = 0; i < oreItems.Count; i++)
        {
            if (i >= oreAmounts.Count) break;

            ResultOreItem item = oreItems[i];
            OreAmount data = oreAmounts[i];

            // 캡처용 지역 변수
            int bonusAmount = data.amount * 5;

            // i번째 아이템은 조금씩 늦게 실행
            DOVirtual.DelayedCall(startDelay + interval * i, () =>
            {
                // 숫자 변경
                item.SetAmount(bonusAmount);

                // 혹시 이전 Tween이 있다면 제거
                item.transform.DOKill();

                // 팝업 애니메이션
                Sequence seq = DOTween.Sequence();

                seq.Append(item.transform.DOScale(1.2f, 0.12f));

                seq.Append(item.transform.DOScale(1f, 0.12f));
            });
        }
    }
}
