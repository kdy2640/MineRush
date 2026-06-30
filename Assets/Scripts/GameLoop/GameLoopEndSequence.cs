using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopEndSequence : MonoBehaviour
{
    [SerializeField] EndUI endUI;
    [SerializeField] ResultUI resultUI;
    [SerializeField] UI_OrePanel orePanel;
    [SerializeField] float earnDealy = 0.1f;
    private List<OreAmount> saveOreList = new();
    private List<OreAmount> diffOreList = new();
    void Start()
    { 
        GameManager.Instance.GameLoop.Events.Subscribe(GameLoopEventType.LoopStarted, OnGameStart);
        GameManager.Instance.GameLoop.Events.Subscribe(GameLoopEventType.LoopEnded, OnGameEnd);
        endUI.gameObject.SetActive(false);
        resultUI.gameObject.SetActive(false);
    }

    private void OnDestroy()
{
    GameManager.Instance.GameLoop.Events.Unsubscribe(GameLoopEventType.LoopStarted, OnGameStart);
    GameManager.Instance.GameLoop.Events.Unsubscribe(GameLoopEventType.LoopEnded, OnGameEnd);
    }
     
    private void OnGameStart()
    {
        saveOreList.Clear();
        for (int i = 0; i < (int)OreType.Length; i++)
        {
            OreType type = (OreType)i; 
            saveOreList.Add(new OreAmount(type,GameManager.Instance.OreManager.GetAmount((OreType)i)));
        }
    }
    private void OnGameEnd()
    {
        diffOreList.Clear(); 
        for (int i = 0; i < (int)OreType.Length; i++)
        {
            OreType type = (OreType)i;
            diffOreList.Add(new OreAmount(type, GameManager.Instance.OreManager.GetAmount((OreType)i)));
            diffOreList[i].amount -= saveOreList[i].amount;
        }
        StartCoroutine(EndGameRoutine());
    }
    private IEnumerator EndGameRoutine()
    {
        endUI.gameObject.SetActive(true);
        yield return endUI.PlayRoutine();
        float rewardMultipler = GameManager.Instance.Upgrade.GetRuntimeStat().RewardMultiplier;
        resultUI.gameObject.SetActive(true);
        resultUI.SetMultiplier(rewardMultipler);
        resultUI.SetData(diffOreList);

        for (int i = 0; i < diffOreList.Count; i++)
        {
            int bonusAmount = Mathf.RoundToInt(diffOreList[i].amount * rewardMultipler) - diffOreList[i].amount;
            GameManager.Instance.OreManager.Add((OreType)i, bonusAmount);
        }

        yield return resultUI.Show();
        yield return resultUI.PlayBonusAnim(diffOreList);

        for (int i = 0; i < diffOreList.Count; i++)
        {  
            orePanel.RefreshOneUI(new OreAmount((OreType)i,1), true);
            yield return new WaitForSeconds(earnDealy);
        }
    }
}
