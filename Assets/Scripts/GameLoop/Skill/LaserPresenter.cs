using System.Collections;
using UnityEngine;

public class LaserPresenter : MonoBehaviour, IAttackPresenter
{
    [SerializeField] SpriteFlipbookPlayer laserFlipbook;
    public IEnumerator PlayAttackRoutine(Vector3 targetPosition)
    {
        transform.position = targetPosition;

        yield return laserFlipbook.PlayRoutine();

        // 여기부터는 플립북 끝난 뒤 로직
    }
}
