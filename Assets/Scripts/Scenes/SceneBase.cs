public abstract class SceneBase
{
    public abstract SceneType SceneType { get; }
    public abstract string SceneName { get; }

    public virtual void Enter() { }
    public virtual void Exit() { }
}