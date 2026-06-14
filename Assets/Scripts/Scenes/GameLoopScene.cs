public class GameLoopScene : SceneBase
{
    public override SceneType SceneType => SceneType.GameLoop;
    public override string SceneName => "GameLoopScene";

    public override void Enter()
    {
        // RuntimeStat 준비
    }

    public override void Exit()
    {
        // 게임 결과 정산
    }
}