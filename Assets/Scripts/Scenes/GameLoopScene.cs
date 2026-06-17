public class GameLoopScene : SceneBase
{
    public override SceneType SceneType => SceneType.GameLoop;
    public override string SceneName => "GameLoopScene";

    public override void Enter()
    {
        // RuntimeStat ???
        //
        GameManager.Instance.SkillManager.ApplySkillsBeforeLoopSceneStart();
        GameManager.Instance.GameLoop.StartLoop();
    }

    public override void Exit()
    { 
        GameManager.Instance.SkillManager.DeactivateSkillsBeforeLoopSceneExit();
    }
}