using Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static Define;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using Random = UnityEngine.Random;

public class ObjectManager
{
    public PlayerController Player { get; private set; }
    public HashSet<MonsterController> Monsters { get; } = new HashSet<MonsterController>();
    public HashSet<CreatureController> Friends { get; } = new HashSet<CreatureController>();

    public ObjectManager()
    {
        Init();
    }

    public void Init()
    {

    }

    public void Clear()
    {
        Monsters.Clear();
    }

    public void LoadMap(string mapName)
    {
        GameObject objMap = Managers.Resource.Instantiate(mapName);
        //objMap.transform.position = new Vector3.zero;
        objMap.name = "@Map";
    }

    public void ShowDamageFont(Vector2 pos, float damage, float healAmount, Transform parent, bool isCritical = false)
    {
        string prefabName;
        if (isCritical)
            prefabName = "CriticalDamageFont";
        else
            prefabName = "DamageFont";

        GameObject go = Managers.Resource.Instantiate(prefabName, pooling: true);
        DamageFont damageText = go.GetOrAddComponent<DamageFont>();
        damageText.SetInfo(pos, damage, healAmount, parent, isCritical);
    }

    public T Spawn<T>(Vector3 position, int templateID = 0, string prefabName = "") where T : BaseController
    {
        System.Type type = typeof(T);

        if (type == typeof(PlayerController))
        {
            GameObject go = Managers.Resource.Instantiate(Managers.Data.CreatureDic[templateID].PrefabLabel);
            PlayerController creature = go.GetOrAddComponent<PlayerController>();
            Vector3 spawnPosition = FindSpawnPosition(ObjectType.Player);

            go.transform.position = spawnPosition;
            creature.InitPos = spawnPosition;
            creature.SetInfo(templateID);
            Player = creature;
            Managers.Game.Player = creature;
            Friends.Add(creature);
            //Managers.Game.CurrentMap.Grid.Add(creature);
            Managers.Game.CurrentMap.Grid.Add(creature);
            return creature as T;
        }
        else if (type == typeof(MonsterController))
        {
            Vector3 originalPos = FindSpawnPosition(ObjectType.Monster);
            Data.CreatureData cd = Managers.Data.CreatureDic[templateID];

            float angleInterval = 360f / cd.CloneCount; // 360도를 몬스터 수로 나눈 각도 간격

            for (int i = 0; i < cd.CloneCount; i++)
            {
                GameObject go = Managers.Resource.Instantiate($"{cd.PrefabLabel}", pooling: true);
                MonsterController creature = go.GetOrAddComponent<MonsterController>();

                Vector3 spawnPosition = originalPos;

                if (i > 0)
                {
                    // 나머지 몬스터는 몬스터를 기준으로 360도로 나눠서 배열
                    float angle = i * angleInterval;
                    Vector3 offset = Quaternion.Euler(0f, 0f, angle) * Vector3.right;
                    spawnPosition += offset * 0.8f;
                }
                go.transform.position = spawnPosition;
                creature.InitPos = spawnPosition;

                creature.SetInfo(templateID);
                go.name = cd.PrefabLabel;
                Monsters.Add(creature);
                Managers.Game.CurrentMap.Grid.Add(creature);

            }
            return null;
        }
        else if (type == typeof(FriendController))
        {
            Vector3 originalPos = FindSpawnPosition(ObjectType.Friend);
            Data.CreatureData cd = Managers.Data.CreatureDic[templateID];

            float angleInterval = 360f / cd.CloneCount; // 360도를 몬스터 수로 나눈 각도 간격

            for (int i = 0; i < cd.CloneCount; i++)
            {
                GameObject go = Managers.Resource.Instantiate($"{cd.PrefabLabel}", pooling: true);
                FriendController creature = go.GetOrAddComponent<FriendController>();

                Vector3 spawnPosition = originalPos;

                if (i > 0)
                {
                    // 나머지 몬스터는 몬스터를 기준으로 360도로 나눠서 배열
                    float angle = i * angleInterval;
                    Vector3 offset = Quaternion.Euler(0f, 0f, angle) * Vector3.right;
                    spawnPosition += offset * 0.8f;
                }
                go.transform.position = spawnPosition;
                creature.InitPos = spawnPosition;
                creature.SetInfo(templateID);
                go.name = cd.PrefabLabel;
                Friends.Add(creature);
                Managers.Game.CurrentMap.Grid.Add(creature);
            }
        }
        return null;
    }

    public void Despawn<T>(T obj) where T : BaseController
    {
        System.Type type = typeof(T);

        if (type == typeof(PlayerController))
        {
            Friends.Remove(obj as PlayerController);
            Managers.Resource.Destroy(obj.gameObject);
            //Managers.Game.CurrentMap.Grid.Remove(obj as PlayerController);
        }

        else if (type == typeof(FriendController))
        {
            Friends.Remove(obj as FriendController);
            Managers.Resource.Destroy(obj.gameObject);
            //Managers.Game.CurrentMap.Grid.Remove(obj as FriendController);

        }
        else if (type == typeof(MonsterController))
        {
            Monsters.Remove(obj as MonsterController);
            Managers.Resource.Destroy(obj.gameObject);

            if (Monsters.Count == 0)
            {
                //TODO Wave END!
                Managers.Game.SetGameState(eGameState.MoveNext);
            }

        }
        else if (type == typeof(BossController))
        {
            Monsters.Remove(obj as MonsterController);
            Managers.Resource.Destroy(obj.gameObject);
            //Managers.Game.CurrentMap.Grid.Remove(obj as MonsterController);
        }

    }

    public bool CheckAllMonsterPosition()
    {
        foreach (MonsterController monster in Monsters.ToList())
        {
            if (monster.transform.position != monster.InitPos)
                return false;
        }

        foreach (CreatureController monster in Friends.ToList())
        {
            if (monster.transform.position != monster.InitPos)
                return false;
        }

        return true;
    }
    public Vector3 FindSpawnPosition(ObjectType type)
    {
        float xMin = Managers.Game.Camera.Left;
        float xMax = Managers.Game.Camera.Right;
        float x = xMin - Define.X_SPAWN_OFFSET;
        float y = 0;
        Vector3 spawnPosition = new Vector3(x, y, 0);

        int n = 0;
        while (true)
        {
            switch (type)
            {
                case ObjectType.Player:
                    x = xMin + Define.X_SPAWN_OFFSET - 1f;
                    y = 2;

                    break;
                case ObjectType.Friend: // 왼쪽에서 소환
                    x = xMin + Random.Range(Define.X_SPAWN_OFFSET, Define.X_SPAWN_OFFSET + 3.0f);
                    y = Random.Range(0f, 6f); // y 좌표 범위 (0부터 6까지의 랜덤한 값)
                    break;
                case ObjectType.Monster:
                case ObjectType.Boss:// 오른쪽에서 소환
                    x = xMax - Random.Range(Define.X_SPAWN_OFFSET, Define.X_SPAWN_OFFSET + 3.0f);
                    y = Random.Range(0f, 6f); // y 좌표 범위 (0부터 6까지의 랜덤한 값)
                    break;
                default:
                    x = xMin - Define.X_SPAWN_OFFSET;
                    break;
            }

            spawnPosition = new Vector3(x, y, 0);
            if (Managers.Game.CurrentMap.Grid.CanAdd(spawnPosition) == true)
            { 
                return spawnPosition;
            }
            n++;
            if (n == 100)
            {
                return spawnPosition;
            }
        }

    }

    public CreatureController FindTarget(CreatureController creature, Vector3 position)
    {
        // 상대 진영에서 가장 가까운놈을 찾자
        switch (creature.ObjectType)
        {
            case ObjectType.Player:
            case ObjectType.Friend: // 왼쪽에서 소환
                // 몬스터중 가장 가까운놈 리턴
                if (Monsters.Count == 0)
                    return null;

                MonsterController closestMonster = Monsters.OrderBy(monster => (position - monster.CenterTrans.position).sqrMagnitude).FirstOrDefault();
                return closestMonster;

            case ObjectType.Monster:
            case ObjectType.Boss:// 오른쪽에서 소환
                                 //플레이어와 친구들중 가장 가까운놈
                if (Friends.Count == 0)
                    return null;
                CreatureController closet = Friends.OrderBy(friend => (position - friend.CenterTrans.position).sqrMagnitude).FirstOrDefault();
                return closet;
        }
        return null;
    }

    public void ClearCreatures()
    {
        foreach (MonsterController monster in Monsters.ToList())
        {
            if (monster.ObjectType == ObjectType.Monster)
                monster.OnDead();
        }

        foreach (CreatureController monster in Friends.ToList())
        {
            //if (monster.ObjectType == ObjectType.Monster)
                monster.OnDead();
        }
    }

    bool IsWithInCamera(Vector3 pos)
    {
        if (pos.x >= 0 && pos.x <= 1 && pos.y >= 0 && pos.y <= 1)
            return true;
        return false;
    }


}
