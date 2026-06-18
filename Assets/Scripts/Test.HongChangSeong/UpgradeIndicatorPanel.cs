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


    [field: SerializeField] public TextMeshProUGUI DisplayNameText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI DescriptionText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI CostText { get; private set; } //이거 OreAmountText로 바꾸는 게?
    [field: SerializeField] public TextMeshProUGUI LevelText { get; private set; }

    private RectTransform rectTransform;
    private Vector3 originalScale;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = transform.localScale;
    }

    private void OnEnable() //SetActive 방식으로 하셨기에 OnEnable을 써봤음.
    {
        transform.DOKill();
        rectTransform.DOScale(originalScale, 0.2f);
    }
}
