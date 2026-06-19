using TMPro;
using UnityEngine;
using DG.Tweening;


//홍창성 0618
//UpgradeNodePanelController에게 프로퍼티들을 제공할 스크립트 + 활성화됐을 때 DOTween을 사용한 효과.
//이 클래스를 필드로 추가하고 필드명.DisplayNameText.text = 이런식으로 하게 되지 않을까.

//이 방식의 장점...?
//UpgradeNodePanelController가 이하의 필드들을 다 갖지 않아도 된다.
public class UpgradeIndicatorPanel : MonoBehaviour
{
    //최종 구현에서는 아마도, 여기에서의 costText와
    //UpgradeNodePanelController의 RefreshDescriptionPanel 메서드 내부가 수정되어야 한다.
    //왜냐하면, 광물은 이미지로 나올 것이고, 가격은 그대로 숫자로 나올 거니까.

    [Header("출력할 부분")]
    [SerializeField] private UpgradeNodePanelController upgradeNodePanelController;// 손유민 : 패널컨트롤러와의 연결을 위한 추가
    [field: SerializeField] public TextMeshProUGUI DisplayNameText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI DescriptionText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI CostText { get; private set; } //이거 OreAmountText로 바꾸는 게?
    [field: SerializeField] public TextMeshProUGUI LevelText { get; private set; }

    [Header("등장 시 패널 크기")]
    [SerializeField] private float hoverScale = 1.2f;
    [Header("연출 지속시간")]
    [SerializeField] private float duration = 0.5f;

    private RectTransform rectTransform;
    private Vector3 originalScale;
    private Vector3 originalRotation;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = transform.localScale;
    }

    private void OnEnable() //SetActive 방식으로 하셨기에 OnEnable을 써봤음.
    {
        rectTransform.DOKill();

        Sequence sequence = DOTween.Sequence();

        //강의에서 시연받았던 방식이 아니라 From을 사용한 방식으로 작성됨.
        sequence.Join(rectTransform.DOScale(originalScale, duration).From(originalScale * hoverScale).SetEase(Ease.OutCubic));

        //sequence.Join(rectTransform.DOShakeRotation(duration, Vector3.forward * 10, 3, 2, true));

        sequence.Join(rectTransform.DOPunchRotation(Vector3.forward * 15f, duration, 6, 0.5f));

        sequence.OnComplete(() => //시퀀스가 종료되면, 명시적으로 회전값 0, 0, 0으로. 오류 방지.
        {
            rectTransform.localRotation = Quaternion.Euler(originalRotation);
        });
    }
    private void OnDisable() //패널 비활성화 될 때
    {
        rectTransform.DOKill(); //해당 패널에 적용된 모든 효과를 끄고
        rectTransform.localRotation = Quaternion.Euler(originalRotation); //rotation 값을 원본으로 돌림.

        //UI는 rotation보다 localRotation을 사용하는 것이 안전하다고 함.
    }
}
