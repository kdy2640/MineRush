using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : MonoBehaviour
{
    [SerializeField] private RectTransform popupContainer;

    [SerializeField] private Button closeButton;
    [SerializeField] private Button toggleButton;
    [SerializeField] private TMP_Text toggleText;

    private AudioManager audioManager;

    [Header("Volume Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Volume % Text")]
    [SerializeField] private TextMeshProUGUI masterVolText;
    [SerializeField] private TextMeshProUGUI bgmVolText;
    [SerializeField] private TextMeshProUGUI sfxVolText;

    private bool isToggleOn = true;

    private float prevMaster;
    private float prevBGM;
    private float prevSFX;

    private void Awake()
    {
        masterSlider.onValueChanged.AddListener(OnMasterChanged);

        bgmSlider.onValueChanged.AddListener(OnBGMChanged);

        sfxSlider.onValueChanged.AddListener(OnSFXChanged);

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(Close);
        }
        if (toggleButton != null)
            toggleButton.onClick.AddListener(Toggle);

        RefreshToggleText();
        UpdateVolumeTexts();
    }
    private void Start()
    {
        audioManager = GameManager.Instance.AudioManager;
    }
    private void OnDestroy()
    {
        masterSlider.onValueChanged?.RemoveListener(OnMasterChanged);
        bgmSlider.onValueChanged?.RemoveListener(OnBGMChanged);
        sfxSlider.onValueChanged?.RemoveListener(OnSFXChanged);

        if (closeButton != null)
            closeButton.onClick.RemoveListener(Close);

        if (toggleButton != null)
            toggleButton.onClick.RemoveListener(Toggle);
    }
    private void UpdateVolumeTexts()
    {
        masterVolText.text = $"{masterSlider.value * 100f:f0}%";
        bgmVolText.text = $"{bgmSlider.value * 100f:f0}%";
        sfxVolText.text = $"{sfxSlider.value * 100:f0}%";
    }
    private void OnMasterChanged(float value)
    {
        if (audioManager == null) return;

        audioManager.SetMasterVolume(value);

        masterVolText.text = $"{masterSlider.value * 100f:f0}%";
    }

    private void OnBGMChanged(float value)
    {
        if (audioManager == null) return;

        audioManager.SetBGMVolume(value);

        bgmVolText.text = $"{bgmSlider.value * 100f:f0}%";
    }

    private void OnSFXChanged(float value)
    {
        if (audioManager == null) return;

        audioManager.SetSFXVolume(value);

        sfxVolText.text = $"{sfxSlider.value * 100:f0}%";
    }
    public void Open()
    {
        popupContainer.DOKill();

        gameObject.SetActive(true);

        popupContainer.localScale = Vector3.zero;

        popupContainer.DOScale(1f, 0.25f).SetEase(Ease.OutBack);

    }
    public void Close()
    {
        popupContainer.DOKill();

        popupContainer.DOScale(0f, 0.2f).SetEase(Ease.InBack).
            OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }
    private void Toggle()
    {
        isToggleOn = !isToggleOn;

        if (!isToggleOn)
        {
            prevMaster = masterSlider.value;
            prevBGM = bgmSlider.value;
            prevSFX = sfxSlider.value;

            masterSlider.value = 0;
            bgmSlider.value = 0;
            sfxSlider.value = 0;
        }
        else
        {
            masterSlider.value = prevMaster;
            bgmSlider.value = prevBGM;
            sfxSlider.value = prevSFX;
        }

        RefreshToggleText();

    }
    private void RefreshToggleText()
    {
        if (toggleText == null) return;

        toggleText.text = isToggleOn ? "ON" : "OFF";
    }
}
