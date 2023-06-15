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
                if (_coWaitStageReady != null)
                {
                    CoroutineManager.StopCoroutine(_coWaitStageReady);
                    _coWaitStageReady = null; // Reset the coroutine reference
                }
                _coWaitStageReady = CoroutineManager.StartCoroutine(CoWaitStageReady());
                break;
            case eGameState.Fight:
                break;
            case eGameState.MoveNext:

                //친구들 InitPos로 이동

                //SpawnMonsters();

                break;
        }
    }
    #endregion

    public void SpawnMonsters()
    {
        CoroutineManager.StartCoroutine(CoMonsterSpawn());
    }

    public void SpawnFriendss()
    {
        CoroutineManager.StartCoroutine(CoFriendsSpawn());
    }

    IEnumerator CoWaitStageReady()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (Managers.Object.CheckAllMonsterPosition() == true) // 애들 다 
            {
                _coWaitStageReady = null;
                SetGameState(eGameState.Fight);
                yield break;
            }
        }
    }
    IEnumerator CoFriendsSpawn()
    {
        Managers.Object.Spawn<PlayerController>(Vector3.zero, Define.PLAYER_DATA_ID);

        Managers.Object.Spawn<FriendController>(Vector3.zero, Define.FRIEND_DATA_ID_1, "Friend1");
        Managers.Object.Spawn<FriendController>(Vector3.zero, Define.FRIEND_DATA_ID_2, "Friend3");
        Managers.Object.Spawn<FriendController>(Vector3.zero, Define.FRIEND_DATA_ID_3, "Friend6");
        Managers.Object.Spawn<FriendController>(Vector3.zero, Define.FRIEND_DATA_ID_4, "Friend6");
        //Managers.Object.Spawn<FriendController>(Vector3.zero, Define.MONSTER_DATA_ID);

        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));

        //Managers.Object.Spawn<FriendController>(Vector3.zero, Define.FRIEND_DATA_ID_3, "Friend7");
        //Managers.Object.Spawn<FriendController>(Vector3.zero, Define.FRIEND_DATA_ID_3, "Friend8");

    }

    IEnumerator CoMonsterSpawn()
    {
        Managers.Object.Spawn<MonsterController>(Vector3.zero, Define.MONSTER_DATA_ID);
        //Managers.Object.Spawn<MonsterController>(Vector3.zero, Define.MONSTER_DATA_ID);
        //Managers.Object.Spawn<MonsterController>(Vector3.zero, Define.MONSTER_DATA_ID);
        //Managers.Object.Spawn<MonsterController>(Vector3.zero, Define.MONSTER_DATA_ID);

        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));

    }
}
