using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreGainPresenter : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] Canvas canvas;
    [SerializeField] OrePresentor UI_OrePrefab; 
    [SerializeField] List<GameObject> OreDummyTarget = new();

    public IEnumerator OreGainRoutine(IReadOnlyList<OreAmount> list, Vector3 spawnPoint)
    {
        // 1. 객체 소환 // pooler 당장은 떔질
        OrePresentor presenter = GameObject.Instantiate(UI_OrePrefab);
        OreType nowType = list[0].oreType;
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
        
        // 4. OrePanel Late 갱신 // OrePanel

        yield return null;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


}
