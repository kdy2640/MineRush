using TMPro;
using UnityEngine;

public class RangeIndicator : MonoBehaviour
{
    public void Initialize(float range)
    {
        transform.localScale =
            Vector3.one * range * 2f;
    }

}
