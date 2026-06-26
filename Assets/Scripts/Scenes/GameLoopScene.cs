using System.Collections;
using UnityEngine;

public class GameLoopScene : SceneBase
{
    public override SceneType SceneType => SceneType.GameLoop;
    public override string SceneName => "GameLoopScene";

    public override IEnumerator PrepareBeforeReveal()
    {
        GameLoopPrepareSequence  PrepareReveal = Object.FindFirstObjectByType<GameLoopPrepareSequence >();

        if (PrepareReveal == null)
        {
            Debug.LogError("GameLoopPrepareReveal가 씬에 없습니다.");
            yield break;
        }

        yield return PrepareReveal.Run();
        yield return null;
    }

    public override IEnumerator Enter()
    {
        GameManager.Instance.SkillManager.ApplySkillsBeforeLoopSceneStart();

        GameLoopPreStart preStart = Object.FindFirstObjectByType<GameLoopPreStart>();

        if (preStart == null)
        {
            Debug.LogError("GameLoopPreStart가 씬에 없습니다.");
            yield break;
        }

        yield return preStart.Run();

        GameManager.Instance.GameLoop.StartLoop();
    }

    public override IEnumerator Exit()
    {
        // GameManager.Instance.GameLoop.StopLoop();
        GameManager.Instance.SkillManager.DeactivateSkillsBeforeLoopSceneExit();

        yield return null;
    }
}