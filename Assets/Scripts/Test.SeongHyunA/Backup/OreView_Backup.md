using System;

using DG.Tweening;

using UnityEngine;



public class OreView : MonoBehaviour

{

&#x20;   \[SerializeField]

&#x20;   private SpriteRenderer sr;



&#x20;   public void PlayHover()

&#x20;   {

&#x20;       transform.DOPunchScale(

&#x20;           Vector3.one \* 0.15f,

&#x20;           0.15f);

&#x20;   }



&#x20;   public void PlayCollect()

&#x20;   {

&#x20;       Sequence seq = DOTween.Sequence();



&#x20;       seq.Append(

&#x20;           transform.DOScale(

&#x20;               0.85f,

&#x20;               0.1f));



&#x20;       seq.Append(

&#x20;           transform.DOShakePosition(

&#x20;               0.2f,

&#x20;               0.15f));

&#x20;   }



&#x20;   public void PlayDisappear(

&#x20;       Action onComplete)

&#x20;   {

&#x20;       sr.DOFade(

&#x20;           0f,

&#x20;           0.3f)

&#x20;           .OnComplete(

&#x20;               () => onComplete?.Invoke());

&#x20;   }



&#x20;   public void ResetView()

&#x20;   {

&#x20;       sr.color = Color.white;



&#x20;       transform.localScale =

&#x20;           Vector3.one;

&#x20;   }

}

