using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MiningEndPopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    private void Awake()
    {
        text.gameObject.SetActive(false);
    }

    public void Play()
    {
        text.gameObject.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(Flow());
    }
    private void OnEnable()
    {
        GameLoopEvents.OnGameEnded += Play;
    }

    private void OnDisable()
    {
        GameLoopEvents.OnGameEnded -= Play;
    }
    private IEnumerator Flow()
    {
        text.alpha = 1f;

        text.transform.localScale =
            Vector3.one * 0.7f;

        Sequence seq = DOTween.Sequence();

        seq.Append( text.transform.DOScale( 1.2f, 0.25f));

        seq.Append( text.transform.DOScale( 1f, 0.15f));

        yield return seq.WaitForCompletion();

        yield return new WaitForSeconds(0.8f);

        text.DOFade(0, 0.3f);

        yield return new WaitForSeconds(0.3f);

        text.gameObject.SetActive(false);
    }
}