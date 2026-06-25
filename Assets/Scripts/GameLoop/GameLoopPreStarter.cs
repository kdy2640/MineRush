using System.Collections;
using UnityEngine;

public class GameLoopPreStart : MonoBehaviour
{ 
    [SerializeField] private StoneSpawner stoneSpawner;
    [SerializeField] private StartUI startUI;

    private void Awake()
    {
        startUI.gameObject.SetActive(false);
    }
    public IEnumerator Run()
    {  
        if (stoneSpawner != null)
           yield return stoneSpawner.PreStartRoutine();

        if (startUI != null)
        {
            startUI.gameObject.SetActive(true);
            yield return startUI.PlayRoutine();
        }
    }
}