using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform statList;
    [SerializeField] private GameObject statItmePrefab;
    [SerializeField] private TextMeshProUGUI totalText;
    [SerializeField] private XPBarUI xpBar;

    [Header("Data")]
    [SerializeField] private StageRewardPanel rewardPanel;

    private void OnEnable()
    {
        StartCoroutine(PlayResult());
    }
    private IEnumerator PlayResult()
    {
        foreach(Transform child in statList)
        {
            Destroy(child.gameObject);
        }
        yield return null;

        int total = 0;

        foreach (OreAmount reward in rewardPanel.Rewards)
        {
            GameObject item = Instantiate(statItmePrefab, statList);

            var ui = item.GetComponent<StatItemUI>();
            ui.SetStatItemUI(null, reward.oreType.ToString(), reward.amount);

            CanvasGroup canvasGroup = item.GetComponent<CanvasGroup>();

            if (canvasGroup == null)
            { 
               canvasGroup = item.AddComponent<CanvasGroup>(); 
            }

            item.transform.localScale = Vector3.one * 0.8f;
            canvasGroup.alpha = 0;

            Sequence seq = DOTween.Sequence();
            seq.Append(canvasGroup.DOFade(1f, 0.25f));
            seq.Join(item.transform.DOScale(1f,0.25f));

            total += reward.amount;

            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.3f);

        yield return StartCoroutine(CountTotal(total));

        xpBar.SetXP(total);
    }
    private IEnumerator CountTotal(int target)
    {
        int current = 0;

        while(current<target)
        {
            current++;
            totalText.text = $"Total\n{current} XP";
            yield return new WaitForSeconds(0.02f);
        }
    }
}

