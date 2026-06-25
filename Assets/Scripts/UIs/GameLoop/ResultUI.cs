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

    [SerializeField] private Button closeButton;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform panel;

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
            });
        }

        if (nextSessionButton != null)
        {
            nextSessionButton.onClick.AddListener(() =>
            {
                PlayButtonAnimation(nextSessionButton);
            });
        }

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(() =>
            {
                PlayButtonAnimation(closeButton);

                CloseResultUI();
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
    public void CloseResultUI()
    {
        Hide();
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

}
