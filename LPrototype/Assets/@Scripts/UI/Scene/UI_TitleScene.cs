using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using static Define;
using UnityEngine.Rendering;

public class UI_TitleScene : UI_Scene
{
    #region Enum
    enum GameObjects
    {
        StartButton
    }

    enum Texts
    {
    }
    #endregion


    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        GetObject((int)GameObjects.StartButton).BindEvent(() => { 
            Managers.Scene.LoadScene(Scene.GameScene); 
        });

        Managers.Resource.LoadAllAsync<Object>("Title", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
            {
                StartLoaded();
            }
        });

        return true;
    }

    void StartLoaded()
    {
        Managers.Data.Init();
    }

}
