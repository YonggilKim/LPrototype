using Data;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using static UnityEngine.GraphicsBuffer;

public class CreatureController : BaseController
{
    public SkeletonAnimation SkeletonAnim;
    public SkeletonDataAsset SkeletonData;
    public CreatureData CreatureData;
    public Transform CenterTrans { get; private set; }
    public virtual int DataId { get; set; }
    public virtual float Hp { get; set; }
    public virtual float MaxHp { get; set; }
    public virtual float HpRegen { get; set; }
    public virtual float Atk { get; set; }
    public virtual float AtkRange { get; set; }
    public virtual float AtkSpeed { get; set; } = 2f;//TODO 데이터 넣기
    public virtual float Def { get; set; }
    public virtual float CriRate { get; set; }
    public virtual float CriDamage { get; set; } = 1.5f;
    public virtual float MoveSpeed { get; set; }
    Define.eCreatureState _creatureState = Define.eCreatureState.Moving;

    public CreatureController Target { get; set; } = null;

    public virtual Define.eCreatureState CreatureState
    {
        get { return _creatureState; }
        set
        {
            _creatureState = value;
            UpdateAnimation();
        }
    }

    public override bool Init()
    {
        base.Init();
        SkeletonAnim = GetComponent<SkeletonAnimation>();
        CenterTrans = transform.GetChild(0);
        return true;
    }

    public void OnEnable()
    {

    }

    public virtual void OnDamaged(BaseController attacker, float damage = 0)
    {
        CreatureController controller = attacker as CreatureController;
        if (controller == null)
            return;

        damage = controller.Atk;
        Hp -= damage;
        Managers.Object.ShowDamageFont(transform.position, damage, 0, transform, false);

        if (Hp <= 0)
        {
            OnDead();
        }
    }

    public virtual void OnDead()
    {
        CreatureState = Define.eCreatureState.Dead;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            CreatureState = eCreatureState.Attack;

        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            CreatureState = eCreatureState.Idle;

        }
    }
    
    public virtual void SetInfo(int creatureId)
    {
        Dictionary<int, Data.CreatureData> dict = Managers.Data.CreatureDic;
        CreatureData = dict[creatureId];
        SkeletonAnim.skeletonDataAsset = Managers.Resource.Load<SkeletonDataAsset>(CreatureData.SkelotonDataID);
        SkeletonAnim.Initialize(true);

        MaxHp = CreatureData.MaxHp;
        Hp = MaxHp;
        Atk = CreatureData.Atk;
        Def = CreatureData.Def;
        MoveSpeed = CreatureData.MoveSpeed;
        AtkRange = CreatureData.AtkRange;

        CreatureState = eCreatureState.Idle;
    }

    public virtual void UpdateAnimation()
    {
        switch (CreatureState)
        {
            case Define.eCreatureState.Idle:
                StopCoroutine(CoIdle());
                StartCoroutine(CoIdle());

                break;
            case Define.eCreatureState.Attack:
                StopCoroutine(AttackTarget());
                StartCoroutine(AttackTarget());
                break;
            case Define.eCreatureState.Skill:
                break;
            case Define.eCreatureState.Moving:
                SkeletonAnim.AnimationState.SetAnimation(0, Define.CREACURE_ANIM_RUN, true);
                StopCoroutine(MoveToTarget());
                StartCoroutine(MoveToTarget());
                break;
            case Define.eCreatureState.Dead:
                break;
        }
    }

    IEnumerator CoIdle()
    {
        while (true)
        {
            if (SkeletonAnim.AnimationName != Define.CREACURE_ANIM_IDLE)
                SkeletonAnim.AnimationState.SetAnimation(0, Define.CREACURE_ANIM_IDLE, true);
            //n초 대기 

            //상대방 지정
            Target = Managers.Object.FindTarget(this, CenterTrans.position);
            if (Target.IsValid() == true)
            {
                CreatureState = eCreatureState.Moving;
                yield break;
            }
            else
            {
                //CreatureState = eCreatureState.Idle;
                yield return new WaitForSeconds(1);
            }
        }
    }

    IEnumerator AttackTarget()
    {
        while (true)
        {
            SkeletonAnim.AnimationState.SetAnimation(0, Define.CREACURE_ANIM_ATTACK, false);
            SkeletonAnim.AnimationState.AddAnimation(0, Define.CREACURE_ANIM_IDLE, false, 0);

            Target = Managers.Object.FindTarget(this, CenterTrans.position);

            if (Target.IsValid() == true)
            {
                Target.OnDamaged(this);
            }
            else
            {
                CreatureState = eCreatureState.Moving;
                yield break;
            }
            yield return new WaitForSeconds(AtkSpeed);
        }
    }

    float _deltaTime = 0;
    float _xScale = 1;
    float _yScale = 0.6f;
    IEnumerator MoveToTarget()
    {
        while (true)
        {
            _deltaTime += Time.deltaTime;
            if (_deltaTime > 0.5f)
            {
                Target = Managers.Object.FindTarget(this, CenterTrans.position);
                _deltaTime = 0;
            }

            if (Target.IsValid() == true)
            {

                float range = (Target.transform.position.x - transform.position.x) > 0 ? AtkRange : AtkRange * -1;
                Vector3 nextPosition = Managers.Game.CurrentMap.Grid.GetNextPosition(Target.transform.position, range);

                //    float newX = transform.position.x + (Target.transform.position.x - transform.position.x) * _xScale;
                //    float newY = transform.position.y + (Target.transform.position.y - transform.position.y) * _yScale;

                //    Vector3 adjustedPosition = new Vector3(newX, newY, Target.transform.position.z);

                //    // 타겟 방향으로 이동
                    Vector3 dir = nextPosition - transform.position;
                dir.Normalize();
                Vector3 newPosition = transform.position + dir * MoveSpeed * Time.deltaTime;
                transform.position = newPosition;

                // AtkRange = UnityEngine.Random.Range(1.5f,2.5f);
                if (Vector3.Distance(transform.position, nextPosition) <= 0.1f)
                {
                    // 공격 범위 안에 들어옴. 공격
                    CreatureState = eCreatureState.Attack;
                    yield break;
                }

                yield return null;
            }
            else
            {
                // Idle로 돌아가서 다시 타겟 찾기
                CreatureState = eCreatureState.Idle;
                yield break;
            }
        }
    }

    //float _deltaTime = 0;
    //IEnumerator MoveToTarget()
    //{
    //    while (true)
    //    {
    //        _deltaTime += Time.deltaTime;
    //        if (_deltaTime > 0.5f)
    //        {
    //            Target = Managers.Object.FindTarget(this, CenterTrans.position);
    //            _deltaTime = 0;
    //        }

    //        if (Target.IsValid() == true)
    //        {
    //            float newY = transform.position.y + (Target.transform.position.y - transform.position.y) * 0.5f;

    //            Vector3 adjustedPosition = new Vector3(Target.transform.position.x, newY, Target.transform.position.z);

    //            //타겟방향으로 이동
    //            Vector3 dir = adjustedPosition - transform.position;
    //            dir.Normalize();
    //            Vector3 newPosition = transform.position + dir * MoveSpeed * Time.deltaTime;
    //            transform.position = newPosition;


    //            //AtkRange = UnityEngine.Random.Range(1.5f,2.5f);
    //            if (Vector3.Distance(transform.position, adjustedPosition) <= AtkRange)
    //            {
    //                if (Mathf.Abs(transform.position.y - Target.transform.position.y) < 0.3f)
    //                {
    //                    //타겟방향으로 y축으로 이동
    //                    continue;
    //                }

    //                //공격범위 안에 들어옴. 공격
    //                CreatureState = eCreatureState.Attack;
    //                yield break;

    //            }
    //            yield return null;
    //        }
    //        else
    //        {
    //            //Idle로 돌아가서 다시 타겟 찾기
    //            CreatureState = eCreatureState.Idle;
    //            yield break;
    //        }
    //    }
    //}

    private void OnDrawGizmos()
    {
        if (Target != null)
        {
            if(ObjectType == ObjectType.Player || ObjectType == ObjectType.Friend)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;
            Gizmos.DrawLine(CenterTrans.position, Target.CenterTrans.position);
        }

        // 디버그 레이 그리기
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawRay(transform.position, transform.forward * 5f);
    }

}
