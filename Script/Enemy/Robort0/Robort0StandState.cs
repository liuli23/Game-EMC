using UnityEngine;

public class Robort0StandState : EnemyState
{
    protected EnemyRobort0 enemyRobort0;
    public Robort0StandState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        enemyRobort0 = enemy as EnemyRobort0;


        enemyRobort0.fx.InvokeRepeating("RedColorBlink", 0, .1f);



        stateTimer = enemyRobort0.standDuration;
        //enemyRobort0.SetVelocity(-enemyRobort0.facingDirection * enemyRobort0.standDirection.x , enemyRobort0.standDirection.y);
        rb.linearVelocity = new Vector2(-enemyRobort0.facingDirection * enemyRobort0.standDirection.x, enemyRobort0.standDirection.y);
    }

    public override void Exit()
    {
        base.Exit();

        enemyRobort0.fx.Invoke("CancelRedBlink",0);
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer<0)
        {
            stateMachine.ChangeState(enemyRobort0.idleState);
        }
    }
}
