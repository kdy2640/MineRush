using System.Collections;
using UnityEngine;

public class MiningSequence : MonoBehaviour
{
    [Header("Actors")]
    [SerializeField] private PickaxeActor pickaxePrefab;
    [SerializeField] private Pooler pickaxePooler;
    // [SerializeField] private OreGainPresenter oreGainPresenter;
    // [SerializeField] private OrePanel orePanel;
      

    private Coroutine currentRoutine;

    private void Awake()
    {
        pickaxePooler = GetComponent<Pooler>();

    }
     

    public void RequestMine(StoneActor stone)
    { 
        if (stone == null)
            return;

        currentRoutine = StartCoroutine(MineRoutine(stone));
    }

    private IEnumerator MineRoutine(StoneActor stone)
    { 
        if (stone.GetComponent<HPHandler>().IsDead)
        { 
            yield break;
        }

        // 1. 데이터 판정 먼저 확정
        stone.GetComponent<HPHandler>().TakeDamage(MiningCalculator.CalculateDamage());

        PickaxeActor pick = pickaxePooler.Get(null).GetComponent<PickaxeActor>();
        // 2. 곡괭이 연출
        yield return pick.PlayAttackRoutine(stone.transform.position);

        // 3. 돌 피격 반응 연출
        yield return stone.GetComponent<StonePresenter>().PlayHitReactionRoutine();

        // 4. 안 죽었으면 여기서 채굴 1회 종료
        if (!stone.GetComponent<HPHandler>().IsDead)
        {
            EndMining();
            yield break;
        }
         
        // 5. 실제 데이터는 즉시 반영
        // 화면에 광석이 아직 날아가는 중이어도, 게임의 진실은 여기서 확정됨.
        GameManager.Instance.OreManager.AddRange(stone.DataSO.RewardList);

        // 6. 돌 파괴 연출
        yield return stone.GetComponent<StonePresenter>().PlayBreakRoutine();

        // 7. 광석 날아가는 연출
        // yield return oreGainPresenter.PlayRoutine(rewardResult, stone.transform.position);

        // 8. UI는 늦게 띠롱
        // OrePanel이 OreManager를 증가시키면 안 됨.
        // 이미 데이터는 위에서 들어갔고, 여긴 표시만 갱신.
        // orePanel.RefreshWithPop(); 

        EndMining();
    }

    private void EndMining()
    { 
        currentRoutine = null;
    }

    private void OnDisable()
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
            currentRoutine = null;
        }
         

        // pickaxe?.StopCurrentTween();
        // oreGainPresenter?.StopCurrentTween();
    }
}