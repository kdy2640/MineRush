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
    [SerializeField] private int vibratio = 3;
    [SerializeField] private float elasticity = 0.25f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void DisplayUpgradeEffect(bool isUpgraded) //매개변수로 bool을 받으면, 이 메서드 하나로 실패/성공 사운드를 둘 다 처리할 수 있을 듯.
    {       
        rectTransform.DOPunchRotation(Vector3.forward * punchPower, duration, vibratio, elasticity);

        if(isUpgraded)
        {
            GameManager.Instance.AudioManager.PlaySFX(SFXType.Upgrade);
        }
        else
        {
            //GameManager.Instance.AudioManager.PlaySFX(SFXType.UpgradeFail);
        }

    }
}
