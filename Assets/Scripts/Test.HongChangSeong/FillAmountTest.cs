using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FillAmountTest : MonoBehaviour
{
    [SerializeField] private Image craftingImage;
    [SerializeField] private float craftingTime = 3.0f; //3초동안 유지해야 fillamount가 1이 되게끔.

    private float holdTime;

    void Update()
    {
        



        if(Input.GetMouseButton(1))
        {
            holdTime += Time.deltaTime;

            craftingImage.fillAmount = Mathf.Clamp01(holdTime / craftingTime);

            if(holdTime >=craftingTime)
            {
                Debug.Log("제작 완료");
                enabled = false;
            }
        }
        else
        {
            holdTime = 0.0f;
            craftingImage.fillAmount = 0f;
        }

    }

    //IEnumerator CraftingPickaxeCo()
    //{
    //    float mouseHold = 0.0f;
        
    //    while (mouseHold < craftingTime)
    //    {
    //        mouseHold += Time.deltaTime;

    //        craftingImage.fillAmount = mouseHold / craftingTime;

    //        yield return null;
    //    }

    //    craftingImage.fillAmount = 1.0f;

    //    Debug.Log("제작완료");
    //}
}
