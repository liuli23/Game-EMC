using UnityEngine;

public class Boss1Skill2State : EnemyState
{
    private EnemyRobort0 enemyRobort0;

    public Boss1Skill2State(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        //Debug.Log("½øÕ½¶·½×¶Î");
        base.Enter();

        enemyRobort0 = enemy as EnemyRobort0;
        enemyRobort0.ZeroVelocity();
        //Debug.Log("¹¥»÷");
    }

    public override void Exit()
    {
        base.Exit();
        enemyRobort0.ZeroVelocity();



        //Debug.Log("²»¹¥»÷");
    }

    public override void Update()
    {
        base.Update();

        enemy.ZeroVelocity();

        if (triggerCalled)
        {
            //Debug.Log("ÇÐ»»Õ½¶·×´Ì¬");
            stateMachine.ChangeState(enemyRobort0.battleState);
        }

    }


}
