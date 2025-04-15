using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyRobort0 : Enemy
{

    //״̬
    #region
    public Robort0IdleState idleState {  get; private set; }
    public Robort0MoveState moveState { get; private set; }
    public Robort0BattleState battleState { get; private set; }
    public Robort0AttackState attackState { get; private set; }
    //public Robort0GroundState groundState { get; private set; }
    public Robort0StandState standState { get; private set; }
    public Robort0DeathState deathState { get; private set; }
    //public GameObject CounterAttackImage;
    public Boss1Skill1State skill1State { get; private set; }
    public Boss1Skill2State skill2State { get; private set; }

    #endregion


    protected override void Awake()
    {
        base.Awake();
        facingDirection = facingDirection * -1;
        idleState = new Robort0IdleState(this, stateMachine, "Idle");
        moveState = new Robort0MoveState(this, stateMachine, "Move");
        battleState = new Robort0BattleState(this, stateMachine, "Move");
        attackState = new Robort0AttackState(this, stateMachine, "Attack");
        //groundState = new Robort0GroundState(this,stateMachine,"")
        standState = new Robort0StandState(this, stateMachine, "Stand");
        deathState = new Robort0DeathState(this, stateMachine, "Death");
        skill1State = new Boss1Skill1State(this, stateMachine, "Skill");
        skill2State = new Boss1Skill2State(this, stateMachine, "Skill2");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);

    }

    protected override void Update()
    {
        base.Update();

        /*
        if(Input.GetKeyDown(KeyCode.U))
        {
            stateMachine.ChangeState(standState);
        }
        *///震刀功能测试


    }


    public void Boss1Skill1() => StartCoroutine(enemyChongci()); // 启动协程


    IEnumerator enemyChongci()
    {
        SetVelocity(70*facingDirection, 0);
        yield return new WaitForSeconds(0.05f);
        ZeroVelocity();

    }

    public override bool CanBeStanded()
    {
        if(base.CanBeStanded())
        {
            stateMachine.ChangeState(standState);
            return true;
        }

        return false;
    }


    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deathState);
    }


}
