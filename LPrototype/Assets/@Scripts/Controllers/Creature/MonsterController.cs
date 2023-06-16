using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : CreatureController
{
    public override bool Init()
    {
        base.Init();
        ObjectType = Define.eObjectType.Monster;

        return true;
    }

    public override void OnDead()
    {
        base.OnDead();
        Managers.Object.Despawn(this);
    }

    public override void SetInfo(int creatureId)
    {
        base.SetInfo(creatureId);
        SkeletonAnim.Skeleton.ScaleX = -1;
    }
    public override void UpdateAnimation()
    {
        if(this.IsValid() == false || Hp < 0) 
        {
            Debug.Log($"{gameObject.name}({ObjectType}) inactive ");
            return;
        }
        base.UpdateAnimation();

        switch (CreatureState)
        {
            case Define.eCreatureState.Idle:
                break;
            case Define.eCreatureState.Skill:
                break;
            case Define.eCreatureState.Moving:
                break;
            case Define.eCreatureState.Dead:
                break;
        }
    }
}
