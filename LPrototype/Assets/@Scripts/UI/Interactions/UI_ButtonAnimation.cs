using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

public class UI_ButtonAnimation : UI_Base
{
    public override bool Init()
    {
        if(_init == true)
            return false;
        gameObject.BindEvent(ButtonPointerDownAnimation, type: Define.UIEvent.PointerDown);
        gameObject.BindEvent(ButtonPointerUpAnimation, type: Define.UIEvent.PointerUp);
        
        return true;
    }

    public void ButtonPointerDownAnimation()
    {
        transform.DOScale(0.85f, 0.1f).SetEase(Ease.InOutBack).SetUpdate(true); 
    }

    public void ButtonPointerUpAnimation()
    {
        transform.DOScale(1f, 0.1f).SetEase(Ease.InOutSine).SetUpdate(true); 
    }


}
