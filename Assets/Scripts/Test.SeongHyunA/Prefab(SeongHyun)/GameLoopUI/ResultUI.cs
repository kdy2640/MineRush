using System.Collections.Generic;
using UnityEngine;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private ResultOreItem itemPrefab;

    [SerializeField] private Transform oreContainer;

    private bool isInitialized;

    private void Awake()
    {
        isInitialized = itemPrefab != null && oreContainer != null;

        if (!isInitialized)
        {
            Debug.LogError( $"[{nameof(ResultUI)}] 초기화 실패");
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

    public void CloseResultUI()
    {
        gameObject.SetActive(false);
    }
}