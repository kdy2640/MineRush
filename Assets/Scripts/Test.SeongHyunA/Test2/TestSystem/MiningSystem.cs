using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiningSystem : MonoBehaviour
{
    [SerializeField] private Camera cam;

    [SerializeField] private float miningRadius = 2f;

    [SerializeField] private LayerMask oreLayer;

    private bool active;

    private float timer;

    private const float RequiredTime = 0.5f;

    public void StartMining()
    {
        active = true;
    }

    public void StopMining()
    {
        active = false;
    }

    private void Update()
    {
        if (!active) return;

        timer += Time.deltaTime;

        if (timer < RequiredTime) return;

        timer = 0f;

        Vector2 mousePos =
            cam.ScreenToWorldPoint(
                Mouse.current.position.ReadValue());

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                mousePos,
                miningRadius,
                oreLayer);

        foreach (Collider2D hit in hits)
        {
            Ore ore = hit.GetComponent<Ore>();

            if (ore == null) continue;

            if (ore.IsMined) continue;

            OreView view = hit.GetComponent<OreView>();

            Mine( ore, view);
        }
    }

    private void Mine(
        Ore ore,
        OreView view)
    {
        ore.SetMined(true);

        //---------------------------------
        // ResultUI용 데이터
        //---------------------------------

        RewardSystem.Instance.Add(
            ore.oreType, 1);

        //---------------------------------
        // 실제 인벤토리 데이터
        //---------------------------------

        OreManager oreManager =
            FindFirstObjectByType<OreManager>();

        if (oreManager != null)
        {
            oreManager.AddRange(
                new List<OreAmount>()
                {
                new OreAmount(
                    ore.oreType,
                    1)
                });
        }

        //---------------------------------
        // 경험치
        //---------------------------------

        XPSystem.Instance.Add(
            ore.xpValue);

        //---------------------------------
        // 연출
        //---------------------------------

        if (view == null)
        {
            ore.gameObject.SetActive(false);
            return;
        }

        view.PlayCollect();

        view.PlayDisappear(
            () =>
            {
                ore.gameObject.SetActive(false);
            });
    }

}