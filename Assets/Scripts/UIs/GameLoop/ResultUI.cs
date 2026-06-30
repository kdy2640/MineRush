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
    //결과보상 배수
    [SerializeField]private float rewardMultiplier = 1.0f;
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
        float startDelay = 0.8f;

        // 각 아이템 사이의 변경 간격
        float interval = 0.2f;

        for (int i = 0; i < oreItems.Count && i < oreAmounts.Count; i++)
        {
            ResultOreItem item = oreItems[i];
            OreAmount data = oreAmounts[i];

            int bonusAmount = Mathf.RoundToInt(data.amount * rewardMultiplier);

            DOVirtual.DelayedCall(startDelay + interval * i, () =>
            {
                // ×5 표시
                item.ShowMultiplier(rewardMultiplier);

                // 지정된 시간 뒤 실제 수량 변경
                DOVirtual.DelayedCall(0.35f, () =>
                {
                    item.SetAmount(bonusAmount);

                    DOVirtual.DelayedCall(0.15f, () =>
                    {
                        item.HideMultiplier();
                    });

                    item.transform.DOKill();

                    Sequence seq = DOTween.Sequence();

                    seq.Append(item.transform.DOScale(1.2f, 0.12f));

                    seq.Append(item.transform.DOScale(1f, 0.12f));
                });
            });
        }
    }
    public void SetMultiplier(float value)
    {
        rewardMultiplier = value;
    }
}
