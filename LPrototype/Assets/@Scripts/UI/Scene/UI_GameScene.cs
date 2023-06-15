using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UI_GameScene : UI_Scene
{
    #region Enum
    enum GameObjects
    {
        FriendsBtn,
        MonsterBtn,
        ClearBtn,
        PlayerBtn
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
        GetObject((int)GameObjects.MonsterBtn).BindEvent(() => 
        {
            Managers.Object.Spawn<MonsterController>(Vector3.zero, Define.MONSTER_DATA_ID);
        });
        GetObject((int)GameObjects.FriendsBtn).BindEvent(() =>
        {
            int dataId = UnityEngine.Random.Range(201002, 201005);
            Managers.Object.Spawn<FriendController>(Vector3.zero, dataId, dataId.ToString());
        });
        GetObject((int)GameObjects.PlayerBtn).BindEvent(() =>
        {
            Managers.Object.Spawn<PlayerController>(Vector3.zero, Define.PLAYER_DATA_ID, "Player");
        });
        GetObject((int)GameObjects.ClearBtn).BindEvent(() =>
        {
            Managers.Object.ClearCreatures();
        });
        return true;
    }

    void StartLoaded()
    {
        Managers.Data.Init();
    }

}
