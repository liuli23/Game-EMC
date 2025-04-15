using UnityEngine;

public class Robort0DeathState : EnemyState
{
    private EnemyRobort0 enemyRobort0;
    public Robort0DeathState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemyRobort0 = enemy as EnemyRobort0;

        enemy.anim.SetBool(enemyRobort0.lastAnimBoolName, true);
        enemy.anim.speed = 0;
        enemy.cd.enabled = false;

        stateTimer = .15f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
        {
            rb.linearVelocity = new Vector2(0, enemy.deathSpeed);
        }
    }
}
