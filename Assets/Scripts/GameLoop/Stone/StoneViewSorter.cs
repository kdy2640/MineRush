using UnityEngine;
 
public class StoneViewSorter : MonoBehaviour
{
    [Header("Renderers")]
    [SerializeField] private SpriteRenderer solidRenderer;
    [SerializeField] private SpriteRenderer[] fragRenderers;
    [SerializeField] private SpriteRenderer crackOverlayRenderer;
     
    private const string StoneLayer = "InGame";

    public void RandomAdjust(float ScaleNoise)
    { 
        transform.localScale = Vector3.one * (1 + (Random.value - 0.5f) * 2 * ScaleNoise); 
        SetFlip(Random.value < 0.5f);

    }
    private void SetFlip(bool isFlip)
    {
        float offset = isFlip ? 1 : -1;
        transform.localScale = new Vector3(offset * transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    public void SetCrack(bool isActive)
    {
        crackOverlayRenderer.gameObject.SetActive(isActive);
    }  

    private void Apply(SpriteRenderer renderer, int order)
    {
        if (renderer == null) return;

        renderer.sortingLayerName = StoneLayer;
        renderer.sortingOrder = order;
    }
}