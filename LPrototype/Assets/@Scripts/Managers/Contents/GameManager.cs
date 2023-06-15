using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public PlayerController Player { get; set; }
    public CameraController Camera { get; set; }
    public Map CurrentMap;
    public void SpawnMonsters()
    {
        CoroutineManager.StartCoroutine(CoMonsterSpawn());
    }

    public void SpawnFriendss()
    {
        CoroutineManager.StartCoroutine(CoFriendsSpawn());
    }

    IEnumerator CoFriendsSpawn()
    {
        Managers.Object.Spawn<FriendController>(Vector3.zero, Define.FRIEND_DATA_ID_1, "Friend1");
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));

        Managers.Object.Spawn<FriendController>(Vector3.zero, Define.FRIEND_DATA_ID_1, "Friend2");
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));

        Managers.Object.Spawn<FriendController>(Vector3.zero, Define.FRIEND_DATA_ID_2, "Friend3");
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));

        Managers.Object.Spawn<FriendController>(Vector3.zero, Define.FRIEND_DATA_ID_2, "Friend4");
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));

        Managers.Object.Spawn<FriendController>(Vector3.zero, Define.FRIEND_DATA_ID_2, "Friend5");
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));

        Managers.Object.Spawn<FriendController>(Vector3.zero, Define.FRIEND_DATA_ID_3, "Friend6");
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));

        Managers.Object.Spawn<FriendController>(Vector3.zero, Define.FRIEND_DATA_ID_3, "Friend7");
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));

        Managers.Object.Spawn<FriendController>(Vector3.zero, Define.FRIEND_DATA_ID_3, "Friend8");
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));

    }

    IEnumerator CoMonsterSpawn()
    {

        Managers.Object.Spawn<MonsterController>(Vector3.zero, Define.MONSTER_DATA_ID);
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));

        Managers.Object.Spawn<MonsterController>(Vector3.zero, Define.MONSTER_DATA_ID);
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));

        Managers.Object.Spawn<MonsterController>(Vector3.zero, Define.MONSTER_DATA_ID);
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));

        Managers.Object.Spawn<MonsterController>(Vector3.zero, Define.MONSTER_DATA_ID);
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));

        Managers.Object.Spawn<MonsterController>(Vector3.zero, Define.MONSTER_DATA_ID);
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));

        Managers.Object.Spawn<MonsterController>(Vector3.zero, Define.MONSTER_DATA_ID);
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));
    }
}
