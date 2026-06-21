using System.Collections.Generic;
using UnityEngine;

public class TestOreManager : MonoBehaviour
{
    public static TestOreManager Instance;

    private readonly List<Ore> ores =
        new List<Ore>();

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterOre(Ore ore)
    {
        if (!ores.Contains(ore))
            ores.Add(ore);
    }

    public void ResetAllOres()
    {
        foreach (Ore ore in ores)
        {
            ore.gameObject.SetActive(true);

            ore.SetMined(false);

            SpriteRenderer sr =
                ore.GetComponent<SpriteRenderer>();

            if (sr != null)
            {
                Color c = sr.color;
                c.a = 1f;
                sr.color = c;
            }

            ore.transform.localScale =
                Vector3.one;
        }
    }
}