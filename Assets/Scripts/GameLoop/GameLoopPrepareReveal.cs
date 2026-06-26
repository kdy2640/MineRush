using System.Collections;
using UnityEngine;

public class GameLoopPrepareReveal : MonoBehaviour
{
    [SerializeField] StoneSpawner spawner;

    public IEnumerator Run()
    {
        yield return spawner.PrepareRoutine();
    }
}
