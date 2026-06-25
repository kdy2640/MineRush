using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Title")]
    [SerializeField] private RectTransform titlePanel;

    [Header("Buttons")]
    [SerializeField] private RectTransform startButton;

    [SerializeField] private RectTransform upgradeButton;

    [SerializeField] private RectTransform settingsButton;

    [SerializeField] private Button startButtonComponent;
    [SerializeField] private Button exitButtonComponent;

    [Header("Popup")]
    [SerializeField] private SettingsPopup settingsPopup;

    [SerializeField] private Button settingsButtonComponent;

    [Header("Animation")]
    [SerializeField] private float titleStartY = 1200f;

    [SerializeField] private float titleEnterDuration = 0.7f;

    private void Awake()
    {
        if (settingsButtonComponent != null)
        {
            settingsButtonComponent.onClick.AddListener(settingsPopup.Open);
        }
        if(startButtonComponent != null)
        {
            startButtonComponent.onClick.AddListener(OnClickStart);
        }
        if (exitButtonComponent != null)
        {
            exitButtonComponent.onClick.AddListener(OnClickExit);
        }
    }

    private void Start()
    {
        StartCoroutine(PlayIntro());
    }

    private IEnumerator PlayIntro()
    {
        titlePanel.anchoredPosition =
            new Vector2(0, titleStartY);

        startButton.localScale = Vector3.zero;
        upgradeButton.localScale = Vector3.zero;
        settingsButton.localScale = Vector3.zero;

        yield return titlePanel
            .DOAnchorPosY( 0, titleEnterDuration)
            .SetEase(Ease.OutBack)
            .WaitForCompletion();

        yield return new WaitForSeconds(0.3f);

        startButton.DOScale(1f, 0.3f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(0.15f);

        upgradeButton.DOScale(1f, 0.3f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(0.15f);

        settingsButton.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
    }
    private void OnClickStart()
    {
        GameManager.Instance.Scene.ChangeScene(SceneType.Upgrade);
    }
    public void OnClickExit()
    {
        Application.Quit();
    }
    private void OnDestroy()
    {
        if(settingsButton != null)
        {
            settingsButtonComponent.onClick.RemoveListener(settingsPopup.Open);
        }
        if(startButtonComponent != null)
        {
            startButtonComponent.onClick.RemoveListener(OnClickStart);
        }
        if (exitButtonComponent != null)
        {
            exitButtonComponent.onClick.RemoveListener(OnClickExit);
        }
    }
}