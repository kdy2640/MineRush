using DG.Tweening;
using UnityEngine;

//0629 홍창성

public class NodeUpgradeEffect : MonoBehaviour
{
    //업그레이드 시도 시 효과음 재생과 업그레이드 노드에 DOTween 효과를 부여할 스크립트.

    //업그레이드 시도 시의 효과는 모두 동일하다. 단, 실패 시와 성공 시의 효과음이 다르다.

    private RectTransform rectTransform;

    [Header("연출 지속시간")]
    [SerializeField] private float duration = 0.5f;

    [Header("효과 조절")]
    [SerializeField] private float punchPower = 5.0f;
    [SerializeField] private int vibratio = 3; //vibrato가 맞긴 한데 라틴어로 vibratio가 있으니 그냥 맞다고 칩시다.
    [SerializeField] private float elasticity = 0.25f;

    //회전을 누적하지 않기 위해 필드를 만들고 메서드 내부에서 값을 할당하는 식으로 한다.
    private Tween punchTween;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }


    //업그레이드 시도 시 DOTween 효과를 실행하고, 업그레이드 성공/실패에 따라 다른 효과음을 재생한다.
    //UpgradeNode 클래스의 TryBuy 메서드 내부에서 호출된다. 매개변수로 bool을 받아 성공/실패 시의 효과를 몰아놓을 수 있다.
    public void DisplayUpgradeEffect(bool isUpgraded)
    {
        punchTween?.Kill(); //punchTween을 끄고
        rectTransform.localRotation = Quaternion.identity; //rotation 값을 원래대로 돌린다.

        punchTween = rectTransform.DOPunchRotation(Vector3.forward * punchPower, duration, vibratio, elasticity);

        if(isUpgraded)
        {
            GameManager.Instance.AudioManager.PlaySFX(SFXType.Upgrade);
        }
        else
        {
            GameManager.Instance.AudioManager.PlaySFX(SFXType.UpgradeFail);
        }
    }
}
