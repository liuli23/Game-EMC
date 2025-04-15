using UnityEngine;

public class Boss1Skill2State : EnemyState
{
    private EnemyRobort0 enemyRobort0;

    public Boss1Skill2State(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        //Debug.Log("��ս���׶�");
        base.Enter();

        enemyRobort0 = enemy as EnemyRobort0;
        enemyRobort0.ZeroVelocity();
        //Debug.Log("����");
    }

    public override void Exit()
    {
        base.Exit();
        enemyRobort0.ZeroVelocity();



        //Debug.Log("������");
    }

    public override void Update()
    {
        base.Update();

        enemy.ZeroVelocity();

        if (triggerCalled)
        {
            //Debug.Log("�л�ս��״̬");
            stateMachine.ChangeState(enemyRobort0.battleState);
        }

    }


}
