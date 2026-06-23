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

    private bool isToggleOn = true;

    private void Awake()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(Close);
        }
        if (toggleButton != null)
            toggleButton.onClick.AddListener(Toggle);

        RefreshToggleText();
    }
    public void Open()
    {
        gameObject.SetActive(true);

        popupContainer.localScale = Vector3.zero;

        popupContainer.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
    }
    public void Close()
    {
        popupContainer.DOScale(0f, 0.2f).SetEase(Ease.InBack).
            OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }
    private void Toggle()
    {
        isToggleOn = !isToggleOn;

        RefreshToggleText();

    }
    private void RefreshToggleText()
    {
        if (toggleText == null) return;

        toggleText.text = isToggleOn ? "ON" : "OFF";
    }
}
