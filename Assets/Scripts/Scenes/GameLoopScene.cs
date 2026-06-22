using System.Collections;
using UnityEngine;

public class GameLoopScene : SceneBase
{
    private Coroutine enterCoroutine;

    public override SceneType SceneType => SceneType.GameLoop;
    public override string SceneName => "GameLoopScene";

    public override void Enter()
    {
        enterCoroutine = GameManager.Instance.StartCoroutine(EnterRoutine());
    }

    private IEnumerator EnterRoutine()
    {
        GameManager.Instance.SkillManager.ApplySkillsBeforeLoopSceneStart();

        var preStart = Object.FindFirstObjectByType<GameLoopPreStart>();

        if (preStart == null)
        {
            Debug.LogError("GameLoopPreStart가 씬에 없습니다.");
            yield break;
        }

        yield return preStart.Run();

        GameManager.Instance.GameLoop.StartLoop();

        enterCoroutine = null;
    }

    public override void Exit()
    {
        if (enterCoroutine != null)
        {
            GameManager.Instance.StopCoroutine(enterCoroutine);
            enterCoroutine = null;
        }

        // GameManager.Instance.GameLoop.StopLoop();
        GameManager.Instance.SkillManager.DeactivateSkillsBeforeLoopSceneExit();
    }
}