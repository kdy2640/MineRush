using System.Collections;
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

    [SerializeField]
    private GameFlowController flow;

    [SerializeField]
    private MonoBehaviour providerObject;

    private IResultProvider provider;

    private bool playing;

    private void Awake()
    {
        provider = providerObject as IResultProvider;

        root.alpha = 0;
        root.blocksRaycasts = false;
        root.interactable = false;

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
        if (miningEndText == null)
            yield break;

        miningEndText.gameObject.SetActive(true);

        miningEndText.text = "MINING END";

        miningEndText.transform.localScale = Vector3.one * 0.7f;

        Sequence seq = DOTween.Sequence();

        seq.Append( miningEndText.transform.DOScale( 1.2f, 0.25f));

        seq.Append( miningEndText.transform.DOScale( 1f, 0.15f));

        yield return seq.WaitForCompletion();

        yield return new WaitForSeconds(0.8f);

        miningEndText.DOFade(0f, 0.3f);

        yield return new WaitForSeconds(0.3f);

        miningEndText.gameObject.SetActive(false);
    }

    private IEnumerator PlayList()
    {
        foreach (Transform child in listParent)
            Destroy(child.gameObject);

        yield return null;

        var data = provider.GetSessionData();

        foreach (var reward in data)
        {
            GameObject obj = Instantiate( itemPrefab, listParent);

            StatItemUI item = obj.GetComponent<StatItemUI>();

            item.SetData( reward.Key, reward.Value,
                provider.GetTotalOre(reward.Key));

            CanvasGroup cg = obj.GetComponent<CanvasGroup>();

            if (cg == null) cg = obj.AddComponent<CanvasGroup>();

            cg.alpha = 0;

            obj.transform.localScale = Vector3.one * 0.8f;

            Sequence seq = DOTween.Sequence();

            seq.Append( cg.DOFade( 1f, 0.2f));

            seq.Join( obj.transform.DOScale( 1f, 0.2f));

            yield return new WaitForSeconds(0.05f);
        }

        sessionText.text =
            $"Session XP : {provider.GetSessionXP()}";

        totalText.text =
            $"Total XP : {provider.GetTotalXP()}";
    }

    public void OnRestart()
    {
        root.blocksRaycasts = false;
        root.interactable = false;

        flow.RestartGame();
    }

    public void OnStatButton()
    {
        if (statPanel != null)
            statPanel.Toggle();
    }

    private void OnDisable()
    {
        playing = false;

        StopAllCoroutines();

        root.alpha = 0;
        root.blocksRaycasts = false;
        root.interactable = false;
    }
}