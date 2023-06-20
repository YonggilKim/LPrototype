using Data;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static Define;

public class CreatureController : BaseController
{
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
    public Vector3 InitPos = Vector3.zero;
    public bool HasArrived { get; private set; }

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
        Managers.Game.OnGameStateChange += HandleGameState;
        return true;
    }

    public void OnEnable()
    {
        HasArrived = false;
        InitPos = Vector3.zero;
    }

    private void OnDestroy()
    {
        if (Managers.Game != null)
            Managers.Game.OnGameStateChange -= HandleGameState;
    }

    public virtual void SetInfo(int creatureId)
    {
        Dictionary<int, Data.CreatureData> dict = Managers.Data.CreatureDic;
        CreatureData = dict[creatureId];
        SkeletonAnim.skeletonDataAsset = Managers.Resource.Load<SkeletonDataAsset>(CreatureData.SkelotonDataID);
        SkeletonAnim.Initialize(true);
        SkeletonAnim.Skeleton.SetSkin(CreatureData.SpineSkinName);

        MaxHp = CreatureData.MaxHp;
        Hp = MaxHp;
        Atk = CreatureData.Atk;
        Def = CreatureData.Def;
        MoveSpeed = CreatureData.MoveSpeed;
        AtkRange = CreatureData.AtkRange;

        if (this.IsValid() == true)
        {
            CreatureState = eCreatureState.Moving;
            StartCoroutine(CoArrangePosition());
        }
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
            CreatureState = eCreatureState.FindingEnermy;
        }
    }

    public virtual void UpdateAnimation()
    {
        switch (CreatureState)
        {
            case Define.eCreatureState.Arrange:
                    PlayAnimation(0, CreatureData.AnimMove, true);
                break;
            case Define.eCreatureState.FindingEnermy:
                //StopCoroutine(CoIdle());
                //StopCoroutine(AttackTarget());
                StopAllCoroutines();
                StartCoroutine(CoIdle());
                break;
            case Define.eCreatureState.Attack:
                StopAllCoroutines();
                //StopCoroutine(AttackTarget());
                StartCoroutine(AttackTarget());
                break;
            case Define.eCreatureState.Skill:
                break;
            case Define.eCreatureState.Moving:
                PlayAnimation(0, CreatureData.AnimMove, true);
                StopCoroutine(MoveToTarget());
                StopAllCoroutines();
                StartCoroutine(MoveToTarget());
                break;
            case Define.eCreatureState.Dead:
                break;
        }
    }


    IEnumerator CoIdle()
    {

        if (SkeletonAnim.AnimationName != CreatureData.AnimIdle)
        {
            PlayAnimation(0, CreatureData.AnimIdle, true);
        }

        while (true)
        {
            //상대방 지정
            if (Managers.Game.GameState == eGameState.Fight)
            {
                Target = Managers.Object.FindTarget(this, CenterTrans.position);
                if (Target.IsValid() == true)
                {
                    CreatureState = eCreatureState.Moving;
                    yield break;
                }
                else
                {
                    yield return null;
                }
            }
            else
            {
                HandleGameState(Managers.Game.GameState);
                yield break;
            }
        }
    }

    IEnumerator AttackTarget()
    {
        while (true)
        {
            if (Managers.Game.GameState == eGameState.Fight)
            {
                if (Target.IsValid() == true)
                {
                    PlayAnimation(0, CreatureData.AnimAttack, false, () => 
                    {
                        OnAttack();
                    });
                    AddAnimation(0, CreatureData.AnimIdle, false, 0);
                }
                else
                {
                    CreatureState = eCreatureState.FindingEnermy;
                    yield break;
                }
            }
            else 
            {
                yield break;
            }
            yield return new WaitForSeconds(AtkSpeed);
        }
    }

    void OnAttack()
    {
        if (Target.IsValid() == true)
        {
            Target.OnDamaged(this);
        }
    }

    IEnumerator CoArrangePosition()
    {
        n = 0;
        while (true)
        {
            n++;
            Vector3 dir = InitPos - transform.position;
            dir.Normalize();
            Vector3 newPosition = transform.position + dir * 3 * Time.deltaTime;
            transform.position = newPosition;

            if (Vector3.Distance(transform.position, InitPos) < 0.1f)
            {
                transform.position = InitPos;
                HasArrived = true;
                Managers.Object.CheckAllCreatureArrived();
                //CreatureState = eCreatureState.Arrange_Wait;
                yield break;
            }

            if (n > 100000)
            {
                yield break;
            }

            yield return null;
        }
    }

    float _deltaTime = 0;
    float _xScale = 1;
    float _yScale = 0.6f;
    int n = 0;
    IEnumerator MoveToTarget()
    {
        while (true)
        {
            if (n == 100000)
            {
                Debug.Log("무한루프");
                yield break;
            }
            _deltaTime += Time.deltaTime;
            if (_deltaTime > 0.5f)
            {
                Target = Managers.Object.FindTarget(this, transform.position);
                _deltaTime = 0;
            }
            if (Target.IsValid() == true && Managers.Game.GameState == eGameState.Fight)
            {
                float newX = transform.position.x + (Target.transform.position.x - transform.position.x) * _xScale;
                float newY = transform.position.y + (Target.transform.position.y - transform.position.y) * _yScale;

                Vector3 adjustedPosition = new Vector3(newX, newY, Target.transform.position.z);

                // 타겟 방향으로 이동
                Vector3 dir = adjustedPosition - transform.position;
                dir.Normalize();
                Vector3 newPosition = transform.position + dir * MoveSpeed * Time.deltaTime;
                transform.position = newPosition;
                if (dir.x > 0)
                    SkeletonAnim.Skeleton.ScaleX = 1;
                else
                    SkeletonAnim.Skeleton.ScaleX = -1;

                // AtkRange = UnityEngine.Random.Range(1.5f,2.5f);
                if (Vector3.Distance(transform.position, adjustedPosition) <= AtkRange)
                {
                    // 공격 범위 안에 들어옴. 공격
                    CreatureState = eCreatureState.Attack;
                    yield break;
                }
            }
            else
            {
                // Idle로 돌아가서 다시 타겟 찾기
                CreatureState = eCreatureState.FindingEnermy;
                yield break;
            }
            yield return null;

        }
    }

    private void OnDrawGizmos()
    {
        if (Target.IsValid())
        {
            if (ObjectType == eObjectType.Player || ObjectType == eObjectType.Friend)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;
            Gizmos.DrawLine(CenterTrans.position, Target.CenterTrans.position);
        }
        else
        {
            //clear
        }


        Gizmos.DrawSphere(InitPos, 0.1f);
        Gizmos.color = new Color(1,1,1,0.5f);
        Gizmos.DrawWireSphere(transform.position, AtkRange);

        Vector3 position = transform.position;
        float height = 1.5f;

        Handles.color = Color.red;

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = 28;
        Handles.Label(position + Vector3.up * height, CreatureState.ToString());
        // 디버그 레이 그리기
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawRay(transform.position, transform.forward * 5f);
    }

    public void HandleGameState(Define.eGameState newState)
    {
        switch (newState)
        {
            case eGameState.Preparation:
                break;
            case eGameState.ArrangeFriends:
                if (ObjectType == eObjectType.Friend || ObjectType == eObjectType.Player)
                {
                    if (this.IsValid() == true)
                    {
                        CreatureState = eCreatureState.Arrange;
                        StartCoroutine(CoArrangePosition());
                    }
                }
                break;
            case eGameState.ArrangeFriends_OK:
                break;
            case eGameState.ArrangeMonster:
                if (ObjectType == eObjectType.Monster || ObjectType == eObjectType.Boss)
                {
                    if (this.IsValid() == true)
                    {
                        CreatureState = eCreatureState.Arrange;
                        StartCoroutine(CoArrangePosition());
                    }
                }
                break;
            case eGameState.ArrangeMonster_OK:
                break;
            case eGameState.Fight:
                CreatureState = eCreatureState.FindingEnermy;
                break;
            case eGameState.FightResult:
                HasArrived = false;
                break;
        }
    }
}
