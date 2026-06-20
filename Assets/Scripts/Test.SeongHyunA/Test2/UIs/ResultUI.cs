using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class ResultUI : MonoBehaviour
{
    [Header("Root")]
    [SerializeField] private CanvasGroup root;

    [Header("Title")]
    [SerializeField] private TextMeshProUGUI miningEndText;

    [Header("List")]
    [SerializeField] private Transform listParent;
    [SerializeField] private GameObject itemPrefab;

    [Header("Summary")]
    [SerializeField] private TextMeshProUGUI sessionText;
    [SerializeField] private TextMeshProUGUI totalText;

    [Header("Panels")]
    [SerializeField] private StatPanelUI statPanel;
   
    [SerializeField] private GameFlowController flow;
    private bool playing;


    private void Awake()
    {
        root.alpha = 0;
        gameObject.SetActive(false);
    }

    public void Show()
    {
        if (playing) return;

        playing = true;

        gameObject.SetActive(true);

        root.alpha = 0;

        root.blocksRaycasts = true;
        root.interactable = true;

        root.DOFade(1f, 0.25f);

        StopAllCoroutines();
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        yield return PlayTitle();
        yield return PlayList();
        playing = false;
    }

    private IEnumerator PlayTitle()
    {
        miningEndText.gameObject.SetActive(true);
        miningEndText.text = "MINING END";

        miningEndText.transform.localScale = Vector3.one * 0.7f;

        Sequence seq = DOTween.Sequence();
        seq.Append(miningEndText.transform.DOScale(1.2f, 0.25f));
        seq.Append(miningEndText.transform.DOScale(1f, 0.15f));

        yield return seq.WaitForCompletion();

        yield return new WaitForSeconds(0.8f);

        miningEndText.DOFade(0, 0.3f);

        yield return new WaitForSeconds(0.3f);

        miningEndText.gameObject.SetActive(false);
    }

    private IEnumerator PlayList()
    {
        foreach (Transform c in listParent)
            Destroy(c.gameObject);

        yield return null;

        var data = RewardSystem.Instance.GetAll();

        int sessionTotal = 0;

        foreach (var d in data)
        {
            GameObject obj = Instantiate(itemPrefab, listParent);

            StatItemUI item = obj.GetComponent<StatItemUI>();
            item.SetData(
                d.Key,
                d.Value,
                RewardSystem.Instance.GetTotalReward(d.Key) );

            sessionTotal += d.Value;

            CanvasGroup cg = obj.GetComponent<CanvasGroup>();
            if (cg == null) cg = obj.AddComponent<CanvasGroup>();

            cg.alpha = 0;
            obj.transform.localScale = Vector3.one * 0.8f;

            Sequence seq = DOTween.Sequence();
            seq.Append(cg.DOFade(1f, 0.2f));
            seq.Join(obj.transform.DOScale(1f, 0.2f));

            yield return new WaitForSeconds(0.05f);
        }

        sessionText.text = $"Session XP : {XPSystem.Instance.GetSessionXP()}";
        totalText.text = $"Total XP : {XPSystem.Instance.GetTotalXP()}";
    }

    // ===== BUTTON =====

    public void OnRestart()
    {
        root.blocksRaycasts = false;
        root.interactable = false;

        flow.RestartGame();
    }

    public void OnStatButton()
    {
        statPanel.Toggle();
    }
}