using System.Collections;
using UnityEngine;

public class GameLoopPreStart : MonoBehaviour
{ 
    [SerializeField] private StoneSpawner stoneSpawner;
    // [SerializeField] private StartUI startUI;

    public IEnumerator Run()
    { 
        yield return null;
        // if (stoneSpawner != null)
           //  yield return stoneSpawner.SpawnRoutine();

        // if (startUI != null) ;
            // yield return startUI.ShowRoutine();
    }
}