using UnityEngine;




public class Robort0IdleState : Robort0GroundState
{
    //private EnemyRobort0 enemyRobort0;
    public Robort0IdleState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        //enemyRobort0 = enemy as EnemyRobort0;

        stateTimer = enemyRobort0.idleTime;
        enemyRobort0.ZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();

        AudioManager.instance.PlaySFX(20, enemyRobort0.transform);
        //Debug.Log("²¥·Å");
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            //Debug.Log("Move");
            stateMachine.ChangeState(enemyRobort0.moveState);
        }

    }
}
