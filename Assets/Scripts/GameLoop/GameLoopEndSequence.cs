using System.Collections;
using UnityEngine;

public class GameLoopEndSequence : MonoBehaviour
{
    [SerializeField] EndUI endUI;
    void Start()
    {
        GameManager.Instance.GameLoop.Events.Subscribe(GameLoopEventType.LoopEnded, OnGameEnd);
        endUI.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.Instance.GameLoop.Events.Unsubscribe(GameLoopEventType.LoopEnded, OnGameEnd);
    }
     

    private void OnGameEnd()
    {
        StartCoroutine(EndGameRoutine());
    }
    private IEnumerator EndGameRoutine()
    {
        endUI.gameObject.SetActive(true);
        yield return endUI.PlayRoutine(); 
    }
}
