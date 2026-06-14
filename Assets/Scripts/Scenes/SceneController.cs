using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Main,
    Upgrade,
    GameLoop
}
// 씬을 전환하기 위한 메소드
// GameManager.Instance.Scene.ChangeScene(SceneType)을 통해 실행해주세요.
public class SceneController : MonoBehaviour
{
    private Dictionary<SceneType, SceneBase> scenes;
    private SceneBase currentScene;

    private void Awake()
    {
        scenes = new Dictionary<SceneType, SceneBase>
        {
            { SceneType.Main, new MainScene() },
            { SceneType.Upgrade, new UpgradeScene() },
            { SceneType.GameLoop, new GameLoopScene() }
        };
        currentScene = scenes[SceneType.Main];
    }

    public void ChangeScene(SceneType nextSceneType)
    {
        StartCoroutine(ChangeSceneRoutine(nextSceneType));
    }
     
    private IEnumerator ChangeSceneRoutine(SceneType nextSceneType)
    {
        currentScene?.Exit();

        SceneBase nextScene = scenes[nextSceneType];

        AsyncOperation operation = SceneManager.LoadSceneAsync(nextScene.SceneName);

        while (!operation.isDone)
            yield return new WaitForSeconds(1f);

        currentScene = nextScene;
        currentScene.Enter();
    }
}