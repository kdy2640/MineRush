using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreGainPresenter : MonoBehaviour
{
    [SerializeField] UI_OrePanel UI_OrePanel;
    [SerializeField] OrePooler pooler;
    [SerializeField] List<GameObject> OreDummyTarget = new();

    [SerializeField] float SpawnDelay = 0.12f;
    [SerializeField] float MoveDelay = 0.3f;

    [Header("Ore Presentation")]
    [SerializeField] int OreAmountPerDummy = 1;
    [SerializeField] float ScaleMultiplier = 1f;
    [SerializeField] float PositionNoisePower = 0.3f;

    private List<int> nowAliveOre = new();

    private void Awake()
    {
        pooler = GetComponent<OrePooler>();

        for (int i = 0; i < (int)OreType.Length; i++)
        {
            nowAliveOre.Add(0);
        }
    }

    public IEnumerator OreGainRoutine(IReadOnlyList<OreAmount> list, Vector3 spawnPoint)
    {
        int amountPerDummy = Mathf.Max(1, OreAmountPerDummy);

        for (int i = 0; i < list.Count; i++)
        {
            OreAmount nowAmount = list[i];
            OreType nowType = nowAmount.oreType;

            nowAliveOre[(int)nowType] += nowAmount.amount;

            int remainAmount = nowAmount.amount;

            while (remainAmount > 0)
            {
                int splitAmount = Mathf.Min(amountPerDummy, remainAmount);
                remainAmount -= splitAmount;

                OreAmount separatedAmount = new OreAmount(nowType, splitAmount);

                StartCoroutine(ApplyRoutine(spawnPoint, separatedAmount));

                yield return new WaitForSeconds(SpawnDelay * Random.value);
            }
        }
    }

    private IEnumerator ApplyRoutine(Vector3 spawnPoint, OreAmount nowAmount)
    {
        OreType nowType = nowAmount.oreType;

        Vector3 noisySpawnPoint = spawnPoint + GetPositionNoise();

        OreArgs args = new OreArgs(noisySpawnPoint, nowType);

        // 1. 객체 소환
        OrePresentor presenter = pooler.Get(args);

        presenter.SetData(nowType, noisySpawnPoint);

        ApplyScaleMultiplier(presenter);

        // 2. 소환 후 팝업
        yield return presenter.PopUpRoutine();

        Vector3 targetPoint = Vector3.zero;

        if ((int)nowType < OreDummyTarget.Count && OreDummyTarget[(int)nowType] != null)
        {
            targetPoint = OreDummyTarget[(int)nowType].transform.position;
        }

        yield return new WaitForSeconds(MoveDelay * Random.value);

        // 3. 빨아들이듯 날아가기
        yield return presenter.MoveToTarget(targetPoint);

        nowAliveOre[(int)nowType] -= nowAmount.amount;

        // 4. OrePanel Late 갱신
        UI_OrePanel.RefreshOneUI(nowAmount, nowAliveOre[(int)nowType] == 0);
    }

    private Vector3 GetPositionNoise()
    {
        if (PositionNoisePower <= 0f)
            return Vector3.zero;

        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        return new Vector3(randomDirection.x, randomDirection.y, 0f) * PositionNoisePower;
    }

    private void ApplyScaleMultiplier(OrePresentor presenter)
    {
        if (presenter == null)
            return;

        presenter.transform.localScale = Vector3.one * ScaleMultiplier;
    }
}