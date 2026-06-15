using TMPro;
using UnityEngine;

public class RangeIndicator : MonoBehaviour
{
    [SerializeField] private float duration = 0.5f;

    public void Initialize(float range)
    {
        transform.localScale =
            Vector3.one * range * 2f;
    }

    private void Start()
    {
        Destroy(gameObject, duration);
    }
}
