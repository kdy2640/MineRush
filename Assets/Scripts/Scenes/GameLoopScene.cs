public class GameLoopScene : SceneBase
{
    public override SceneType SceneType => SceneType.GameLoop;
    public override string SceneName => "GameLoopScene";

    public override void Enter()
    {
        // RuntimeStat ¡ÿ∫Ò
        //
        GameManager.Instance.GameLoop.StartLoop();
    }

    public override void Exit()
    { 
    }
}