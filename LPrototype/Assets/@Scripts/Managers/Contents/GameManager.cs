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
    public eGameState CurrentState { get; set; }
    public event Action<eGameState> OnGameStateChange;

    #region GameState
    public void SetGameState(eGameState newState)
    {
        CurrentState = newState;
        OnGameStateChange?.Invoke(CurrentState);
        HandleGameState();
    }

    Coroutine _coWaitStageReady;
    private void HandleGameState()
    {
        switch (CurrentState)
        {
            case eGameState.StageReady:
                SpawnFriends();
                SpawnMonsters();
                if (_coWaitStageReady != null)
                {
                    CoroutineManager.StopCoroutine(_coWaitStageReady);
                    _coWaitStageReady = null; // Reset the coroutine reference
                }
                _coWaitStageReady = CoroutineManager.StartCoroutine(CoWaitStageReady(isAll : true));
                break;
            case eGameState.Fight:
                break;
            case eGameState.MoveNext:
                if (_coWaitStageReady != null)
                {
                    CoroutineManager.StopCoroutine(_coWaitStageReady);
                    _coWaitStageReady = null; // Reset the coroutine reference
                }
                _coWaitStageReady = CoroutineManager.StartCoroutine(CoWaitStageReady(eObjectType.Player));
                break;
            case eGameState.MonsterSpawn:
                if (_coWaitStageReady != null)
                {
                    CoroutineManager.StopCoroutine(_coWaitStageReady);
                    _coWaitStageReady = null; // Reset the coroutine reference
                }
                SpawnMonsters();
                _coWaitStageReady = CoroutineManager.StartCoroutine(CoWaitStageReady(eObjectType.Monster));
                break;
        }
    }
    #endregion

    public void SpawnMonsters()
    {
        CoroutineManager.StartCoroutine(CoMonsterSpawn());
    }

    public void SpawnFriends()
    {
        CoroutineManager.StartCoroutine(CoFriendsSpawn());
    }

    IEnumerator CoWaitStageReady(eObjectType objectType = eObjectType.Monster, bool isAll = false)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (Managers.Object.CheckCreaturePosition(objectType, isAll) == true) // ¾Öµé ´Ù 
            {
                _coWaitStageReady = null;
                if(CurrentState  == eGameState.StageReady)
                {
                    SetGameState(eGameState.Fight);
                }
                else if (CurrentState == eGameState.MoveNext)
                {
                    SetGameState(eGameState.MonsterSpawn);
                }
                else if (CurrentState == eGameState.MonsterSpawn)
                {
                    SetGameState(eGameState.Fight);
                }
                yield break;
            }
        }
    }

    IEnumerator CoFriendsSpawn()
    {
        if (Managers.Object.Friends.Count > 0)
            yield break;

        Managers.Object.Spawn<FriendController>(Vector3.zero, Define.FRIEND_DATA_ID_1);
        Managers.Object.Spawn<FriendController>(Vector3.zero, Define.FRIEND_DATA_ID_1);
        Managers.Object.Spawn<FriendController>(Vector3.zero, Define.PLAYER_DATA_ID);
        Managers.Object.Spawn<FriendController>(Vector3.zero, Define.PLAYER_DATA_ID);
        //Managers.Object.Spawn<PlayerController>(Vector3.zero, Define.PLAYER_DATA_ID);
        //Managers.Object.Spawn<FriendController>(Vector3.zero, Define.FRIEND_DATA_ID_1, "Friend1");

        //Managers.Object.Spawn<FriendController>(Vector3.zero, Define.FRIEND_DATA_ID_1, "Friend1");

        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));

    }

    IEnumerator CoMonsterSpawn()
    {
        Managers.Object.Spawn<MonsterController>(Vector3.zero, Define.MONSTER_DATA_ID);
        Managers.Object.Spawn<MonsterController>(Vector3.zero, Define.MONSTER_DATA_ID);
        Managers.Object.Spawn<MonsterController>(Vector3.zero, Define.MONSTER_DATA_ID);
        Managers.Object.Spawn<MonsterController>(Vector3.zero, Define.MONSTER_DATA_ID_1);
        //Managers.Object.Spawn<MonsterController>(Vector3.zero, Define.MONSTER_DATA_ID);
        //Managers.Object.Spawn<MonsterController>(Vector3.zero, Define.MONSTER_DATA_ID_1);
        //Managers.Object.Spawn<MonsterController>(Vector3.zero, Define.MONSTER_DATA_ID_1); 

        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));

    }
}
