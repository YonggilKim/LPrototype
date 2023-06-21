using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Define;

public class GoldController : BaseController
{

    private void OnDisable()
    {
        Managers.Game.OnGameStateChange -= HandleGameState;
    }

    private void OnEnable()
    {
        Managers.Game.OnGameStateChange += HandleGameState;
    }

    //public void temp()
    //{
    //    Vector2 scrennPos = RectTransformUtility.WorldToScreenPoint(Camera.main, _imageTop.rectTransform.position);
    //    Vector3 pos = Vector3.zero;

    //    RectTransformUtility.ScreenPointToWorldPointInRectangle(_imageTop.rectTransform, scrennPos, Camera.main, out pos);
    //    Debug.Log(scrennPos);
    //    _trmSphere.position = pos;
    //}

    public void MoveToUI()
    {
        RectTransform targetUI = Managers.UI.GetSceneUI<UI_GameScene>().GoldUI;
        Vector3 result = Util.ScreenPointToWorldPoint(targetUI);

        transform.DOMove(result, 1).SetEase(Ease.InOutQuad)
            .OnComplete(() => {
                Managers.Game.Gold += 10;
                Managers.Object.Despawn<GoldController>(this);
            });
    }

    private void HandleGameState(Define.eGameState eGameState)
    {
        switch (eGameState)
        {
            case eGameState.FightResult:
                MoveToUI();


                break;
        }
    }
}
