public class UpgradeScene : SceneBase
{
    public override SceneType SceneType => SceneType.Upgrade;
    public override string SceneName => "UpgradeScene";

    public override void Enter()
    {
        // 업그레이드 씬 진입 시 데이터 준비
    }

    public override void Exit()
    {
        // 업그레이드 결과 확정
        // RuntimeStat 계산 준비
    }
}