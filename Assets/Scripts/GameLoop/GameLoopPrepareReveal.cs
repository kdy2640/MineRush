using System.Collections;
using UnityEngine;

public class GameLoopPrepareSequence  : MonoBehaviour
{
    [SerializeField] StoneSpawner spawner; 
    [SerializeField] PickaxePooler pickaxePooler;
    [SerializeField] OrePooler orePooler; 
    [SerializeField] int pickaxePrewarmCount = 100;
    [SerializeField] int orePrewarmCount = 100;
    public IEnumerator Run()
    {
        pickaxePooler.Prewarm(pickaxePrewarmCount);
        orePooler.Prewarm(orePrewarmCount);
        yield return spawner.PrepareRoutine();
    }
}
