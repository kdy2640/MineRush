using UnityEngine;

public class Ore : MonoBehaviour
{
    public string oreType;
    public int xpValue;

    private bool mined;

    public bool IsMined => mined;
    private void Start()
    {
        if (TestOreManager.Instance != null)
            TestOreManager.Instance.RegisterOre(this);
    }
    public void SetMined(bool value)
    {
        mined = value;
    }
}