using UnityEngine;



public class Robort0MoveState : Robort0GroundState
{
    //private EnemyRobort0 enemyRobort0;
    public Robort0MoveState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //enemyRobort0 = enemy as EnemyRobort0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemyRobort0.SetVelocity(enemyRobort0.moveSpeed * enemyRobort0.facingDirection, rb.linearVelocity.y);
        if(enemyRobort0.IsWalling()||!enemyRobort0.IsGrounding())
        {
            enemyRobort0.Flip();
            //Debug.Log("×ªÏò");
            stateMachine.ChangeState(enemyRobort0.idleState);
        }
    }
}
