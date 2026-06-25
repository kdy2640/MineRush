using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreGainPresenter : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] Canvas canvas;
    [SerializeField] OrePresentor UI_OrePrefab;
    [SerializeField] UI_OrePanel UI_OrePanel;
    [SerializeField] List<GameObject> OreDummyTarget = new();
    [SerializeField] float SpawnDelay = 0.12f;

    private List<int> nowAliveOre = new();
    private void Awake()
    {
        for (int i = 0; i < (int)OreType.Length; i++)
        {
            nowAliveOre.Add(0);
        }
    }
    public IEnumerator OreGainRoutine(IReadOnlyList<OreAmount> list, Vector3 spawnPoint)
    {
        for (int i = 0; i < list.Count; i++)
        { 
            OreAmount nowAmount = list[i];
            OreType nowType = nowAmount.oreType;
            nowAliveOre[(int)nowType] += nowAmount.amount;
            OreAmount seperateAmount = new OreAmount(nowType, 1);
            for (int j = 0; j < nowAmount.amount; j++)
            {
                StartCoroutine(ApplyRoutine(spawnPoint, seperateAmount));
                yield return new WaitForSeconds(SpawnDelay * Random.value);
            } 
        }
    }

    private IEnumerator ApplyRoutine(Vector3 spawnPoint, OreAmount nowAmount)
    {
        OreType nowType = nowAmount.oreType;
        // 1. 객체 소환 // pooler 당장은 떔질
        OrePresentor presenter = GameObject.Instantiate(UI_OrePrefab);

        presenter.SetData(nowType, spawnPoint);

        // 2. 소환 후 팝업 // OrePresenter 
        yield return presenter.PopUpRoutine();

        Vector3 targetPoint = Vector3.zero;

        if ((int)nowType < OreDummyTarget.Count)
        {
            targetPoint = OreDummyTarget[(int)nowType].transform.position;
        }


        // 3. 빨아들이듯 날아가기 // OrePresenter
        yield return presenter.MoveToTarget(targetPoint);

        nowAliveOre[(int)nowAmount.oreType] -= 1;

        // 4. OrePanel Late 갱신 // OrePanel
        UI_OrePanel.RefreshOneUI(nowAmount, nowAliveOre[(int)nowAmount.oreType] == 0);
    }


}
