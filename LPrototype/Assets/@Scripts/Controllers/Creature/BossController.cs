using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : CreatureController
{
    public override void UpdateAnimation()
    {
        base.UpdateAnimation();

        switch (CreatureState)
        {
            case Define.eCreatureState.FindingEnermy:
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
