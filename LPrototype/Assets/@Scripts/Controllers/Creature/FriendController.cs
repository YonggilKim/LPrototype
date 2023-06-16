using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendController : CreatureController
{
    public override bool Init()
    {
        base.Init();
        ObjectType = Define.eObjectType.Friend;
        return true;
    }

    public override void OnDead()
    {
        base.OnDead();
        Managers.Object.Despawn(this);
    }


    public override void UpdateAnimation()
    {
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
