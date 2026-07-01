using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackPresenter
{
    public IEnumerator PlayAttackRoutine(Vector3 targetPosition);
}
public enum MiningType
{
    Pickaxe,
    Laser,
    Bomb,
    Length
}
public class MiningSequence : MonoBehaviour
{
    [Header("Actors")] 
    [SerializeField] private PickaxePooler pickaxePooler;
    [SerializeField] private OreGainPresenter oreGainPresenter;
    [SerializeField] private BombPresenter bombPrefab;
    [SerializeField] private LaserPresenter laserPrefab;
    // [SerializeField] private OrePanel orePanel;
      
     delegate IEnumerator MineRoutine(List<StoneActor> stones, Vector3 targetPosition, MiningType type);
     
    private void Awake()
    {
        pickaxePooler = GetComponent<PickaxePooler>();

    }
     
    public void RequestMines(List<StoneActor> stones, Vector3 attackPosition, MiningType type)
    { 
        if (stones == null)
            return;
        MineRoutine nowRoutine = GetRoutineType(type); 
        if(nowRoutine == null)
        {
            Debug.LogError("올바른 MiningType이 아닙니다.");
            return;
        }

        StartCoroutine(nowRoutine(stones,attackPosition, type));
    } 
    // 공격은 하나 맞는 돌은 여러개
    private IEnumerator MineMultiTargetRoutines(List<StoneActor> stones, Vector3 attackPosition , MiningType type)
    {
        // 1. 데이터 판정 먼저 확정
        for (int i = 0; i < stones.Count; i++)
        { 
            HPHandler stoneHP = stones[i].HP;
            if (stoneHP.IsDead)
            {
                continue;
            }
            stoneHP.TakeDamage(MiningCalculator.CalculateDamage(type));
        }
        IAttackPresenter attack = GetAttackPresenter(type);
        // 2. 공격물 연출
        yield return attack.PlayAttackRoutine(attackPosition);

        // 3. 스톤 타격 루틴
        for (int i = 0; i < stones.Count; i++)
        {
            StartCoroutine(StoneHitSequence(stones[i]));
        } 
    }
    // 공격은 여러개 맞는 돌은 일대일대응
    private IEnumerator MineSingleTargetRoutines(List<StoneActor> stones, Vector3 attackPosition, MiningType type)
    {
        for (int i = 0; i < stones.Count; i++)
        {
            StartCoroutine(MineSingleTargetRoutine(stones[i], type));
        }
        yield return null;
    }

    // 공격은 하나 맞는 돌도 하나
    private IEnumerator MineSingleTargetRoutine(StoneActor stone, MiningType type)
    {
        HPHandler stoneHP = stone.HP;
        if (stoneHP.IsDead)
        {
            yield break;
        }

        // 1. 데이터 판정 먼저 확정
        stoneHP.TakeDamage(MiningCalculator.CalculateDamage(type));


        IAttackPresenter attack = GetAttackPresenter(type);
        // 2. 공격물 연출
        yield return attack.PlayAttackRoutine(stone.transform.position);

        // 3. 스톤 타격 루틴
        yield return StoneHitSequence(stone);
    } 
    private IEnumerator StoneHitSequence(StoneActor stone)
    {
        if(!stone.HP.IsDead)
        { 
            // 3. 돌 피격 반응 연출
            yield return stone.Presenter.PlayHitReactionRoutine();

            stone.CheckCrack();
            yield return null;
        }

        // 4. 진행해야하는지 확인
        if (!stone.TryStartDeathSequence())
            yield break;

        // 5. 실제 데이터는 즉시 반영
        // 화면에 광석이 아직 날아가는 중이어도, 데이터는 여기서 확정됨.
        GameManager.Instance.OreManager.AddRange(stone.DataSO.RewardList);

        // 7. 광석 날아가는 연출 + 광석 습득 피드백 반영 -> 7인데 돌 파괴 연출 딜레이를 위해서 먼저 호출.
        oreGainPresenter.OreGainRoutineCO(stone.DataSO.RewardList, stone.transform.position);

        // 6. 돌 파괴 연출
        yield return stone.Presenter.PlayBreakRoutine();
         
    }

    private IAttackPresenter GetAttackPresenter(MiningType type)
    {

        switch (type)
        {
            case MiningType.Pickaxe:
                return pickaxePooler.Get(null).GetComponent<PickaxeActor>();
            case MiningType.Laser:
                return GameObject.Instantiate(laserPrefab).GetComponent<LaserPresenter>();
            case MiningType.Bomb:
                return GameObject.Instantiate(bombPrefab).GetComponent<BombPresenter>();
        }
        Debug.LogError("Error : Invalid MiningType");
        return pickaxePooler.Get(null).GetComponent<PickaxeActor>();
    }

    private MineRoutine GetRoutineType(MiningType type)
    {

        switch (type)
        {
            case MiningType.Pickaxe:
                return MineSingleTargetRoutines;
            case MiningType.Laser:
                return MineMultiTargetRoutines;
            case MiningType.Bomb:
                return MineMultiTargetRoutines;
            case MiningType.Length:
                break;
        }
        return null;
    }

    private void OnDisable()
    {  
    }
}