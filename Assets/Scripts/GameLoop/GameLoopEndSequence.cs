using System.Collections;
using UnityEngine;

public class GameLoopEndSequence : MonoBehaviour
{
    [SerializeField] EndUI endUI;
    void Start()
    {
        GameManager.Instance.GameLoop.Events.Subscribe(GameLoopEventType.LoopEnded, OnGameEnd);
    }

    private void OnDestroy()
    {
        GameManager.Instance.GameLoop.Events.Unsubscribe(GameLoopEventType.LoopEnded, OnGameEnd);
    }

    private void OnGameEnd()
    {
        EndGameRoutine();
    }
    private IEnumerator EndGameRoutine()
    {
        yield return endUI.PlayRoutine(); 
    }
}
