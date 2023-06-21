using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using static UnityEngine.GraphicsBuffer;

public class GameManager
{
    public PlayerController Player { get; set; }
    public CameraController Camera { get; set; }
    public Map CurrentMap;
    public event Action<eGameState> OnGameStateChange;
    public event Action<int> OnGoldChange;

    int _gold;
    public int Gold
    {
        get
        {
            return _gold;
        }
        set
        {
            _gold = value;
            OnGoldChange?.Invoke(_gold);

        }
    }

    eGameState _currentState;
    public eGameState GameState 
    {
        get
        {
            return _currentState;
        }
        set
        {
            _currentState = value;
            OnGameStateChange?.Invoke(GameState);
            HandleGameState();
        }
    }


    #region GameState

    private void HandleGameState()
    {
        switch (GameState)
        {
            case eGameState.Preparation:
                SpawnFriends();
                GameState = eGameState.ArrangeFriends;
                break;
            case eGameState.ArrangeFriends:
                break;
            case eGameState.ArrangeFriends_OK:
                SpawnMonsters();
                GameState = eGameState.ArrangeMonster;
                break;
            case eGameState.ArrangeMonster:
                break;
            case eGameState.ArrangeMonster_OK:
                GameState = eGameState.Fight;
                break;
            case eGameState.Fight:
                break;
            case eGameState.FightResult:
                GameState = eGameState.ArrangeFriends;
                break;
        }
    }
    #endregion

    public void SpawnMonsters()
    {
        Managers.Object.Spawn<MonsterController>(Vector3.zero, Define.MONSTER_DATA_ID);
        Managers.Object.Spawn<MonsterController>(Vector3.zero, Define.MONSTER_DATA_ID);
        Managers.Object.Spawn<MonsterController>(Vector3.zero, Define.MONSTER_DATA_ID);
        Managers.Object.Spawn<MonsterController>(Vector3.zero, Define.MONSTER_DATA_ID_1);
    }

    public void SpawnFriends()
    {
        if (Managers.Object.Friends.Count > 0)
            return;
        Managers.Object.Spawn<FriendController>(Vector3.zero, Define.FRIEND_DATA_ID_1);
        Managers.Object.Spawn<FriendController>(Vector3.zero, Define.FRIEND_DATA_ID_1);
        Managers.Object.Spawn<FriendController>(Vector3.zero, Define.PLAYER_DATA_ID);
        Managers.Object.Spawn<FriendController>(Vector3.zero, Define.PLAYER_DATA_ID);
    }

}
