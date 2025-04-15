using System.Collections;
using UnityEngine;

public class Boss1Skill1State : EnemyState
{
    private EnemyRobort0 enemyRobort0;

    public Boss1Skill1State(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        //Debug.Log("��ս���׶�");
        base.Enter();
        
        enemyRobort0 = enemy as EnemyRobort0;
        enemyRobort0.ZeroVelocity();
        //Debug.Log("���ܹ���");
    }

    public override void Exit()
    {
        base.Exit();
        enemyRobort0.ZeroVelocity();

        enemy.lastTimeSkill = Time.time;

        //Debug.Log("������");
    }

    public override void Update()
    {
        base.Update();

        //enemy.ZeroVelocity();

        if (triggerCalled)
        {
            //Debug.Log("�л�ս��״̬");

            if(Random.Range(1,100)>50)
                stateMachine.ChangeState(enemyRobort0.skill2State);
            else
                stateMachine.ChangeState(enemyRobort0.battleState);
        }

    }

    //public void Boss1Skill1() => StartCoroutine(enemyChongci()); // ����Э��
    //��Mono

    //IEnumerator enemyChongci()
    //{
    //    enemyRobort0.SetVelocity(30, 0);
    //    yield return new WaitForSeconds(0.4f);
    //    enemyRobort0.ZeroVelocity();
    //
    //}
    //




}
