using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform rewardContent;
    [SerializeField] private GameObject rewardItemPrefab;
    [SerializeField] private TextMeshProUGUI totalText;

    [Header("Data")]
    [SerializeField] private StageRewardPanel rewardPanel;

    [Header("EXP")]
    [SerializeField] private XPBarUI xpBar;

    private void OnEnable()
    {
        StartCoroutine(ShowResultRoutine());
    }

    private IEnumerator ShowResultRoutine()
    {
        xpBar.ResetXP();

        foreach (Transform child in rewardContent)
        {
            Destroy(child.gameObject);
        }

        totalText.text = "";

        yield return null; 

        int total = 0;

        foreach (OreAmount reward in rewardPanel.Rewards)
        {
            GameObject item =
                Instantiate(rewardItemPrefab, rewardContent);

            TextMeshProUGUI txt =
                item.GetComponentInChildren<TextMeshProUGUI>();

            txt.text = $"{reward.oreType} : {reward.amount}";

            RectTransform rect = item.GetComponent<RectTransform>();
            CanvasGroup cg = item.GetComponent<CanvasGroup>();

            if (cg == null)
                cg = item.AddComponent<CanvasGroup>();

            item.transform.localScale = Vector3.one * 0.8f;
            cg.alpha = 0f;

            Sequence seq = DOTween.Sequence();

            seq.Append(cg.DOFade(1f, 0.25f));
            seq.Join(item.transform.DOScale(1f, 0.25f));

            seq.SetEase(Ease.OutBack);

            total += reward.amount;

            yield return new WaitForSeconds(0.15f);
        }

        yield return new WaitForSeconds(0.3f);

        ShowTotal(total);
        xpBar.SetXP(total);
    }

    private void ShowTotal(int total)
    {
        totalText.text = $"Total : {total}";

        CanvasGroup cg =
            totalText.GetComponent<CanvasGroup>();

        if (cg == null)
            cg = totalText.gameObject.AddComponent<CanvasGroup>();

        RectTransform rect = totalText.rectTransform;

        rect.localScale = Vector3.one * 0.8f;
        cg.alpha = 0f;

        Sequence seq = DOTween.Sequence();

        seq.Append(cg.DOFade(1f, 0.4f));
        seq.Join(rect.DOScale(1f, 0.4f));

        seq.SetEase(Ease.OutBack);
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}