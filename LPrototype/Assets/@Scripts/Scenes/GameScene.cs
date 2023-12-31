﻿using Data;
using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

public class GameScene : BaseScene
{
    private void Awake()
    {
        Init();
    }

    protected override void Init()
    {
        Debug.Log("@>> GameScene Init()");
        base.Init();
        SceneType = Define.Scene.GameScene;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //Managers.UI.ShowSceneUI<UI_Joystick>();
    }

    private void Start()
    {
        Managers.Object.LoadMap(Define.STAGE_ID);

        Managers.UI.ShowSceneUI<UI_GameScene>();
        Managers.Game.GameState = eGameState.Preparation;

        //Managers.Object.Spawn<MonsterController>(Vector3.zero, MONSTER_DATA_ID);

    }

    public override void Clear()
    {

    }

}
