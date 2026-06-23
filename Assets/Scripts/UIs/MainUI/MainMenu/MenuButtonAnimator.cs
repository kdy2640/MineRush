using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MenuButtonAnimator : MonoBehaviour
{
    [SerializeField] private float punchScale = 0.15f;

    [SerializeField] private float duration = 0.4f;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(PlayAnimation);
    }

    private void PlayAnimation()
    {
        transform.DOKill();

        transform.DOPunchScale( Vector3.one * punchScale, duration, 8, 0.5f);
    }
}