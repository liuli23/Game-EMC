using UnityEngine;

public class Robort0BattleState : EnemyState
{
    private Transform player;
    private int moveDirection;
    private EnemyRobort0 enemyRobort0;
    public Robort0BattleState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemyRobort0 = enemy as EnemyRobort0;
        //player = GameObject.Find("Player").transform;//改为使用玩家管理器
        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
            stateMachine.ChangeState(enemyRobort0.moveState);

        stateTimer = enemy.battleTime;
    }


    public override void Update()
    {
        base.Update();

        if (player.position.x > enemyRobort0.transform.position.x)
            moveDirection = 1;
        else if (player.position.x < enemyRobort0.transform.position.x)
            moveDirection = -1;

        enemyRobort0.SetVelocity(enemyRobort0.moveSpeed * moveDirection, rb.linearVelocity.y);


        if (enemy.IsPlayering())
        {
            stateTimer = enemy.battleTime;

            if (enemy.IsPlayering().distance < enemy.attackDistance)
            {
                enemy.ZeroVelocity();
                if (CanAttack())
                {
                    if(enemyRobort0.canUseSkill && enemyRobort0.Boss1CanUseSkill())
                        if(Random.Range(1,100)>40)
                            enemyRobort0.stateMachine.ChangeState(enemyRobort0.skill1State);
                        else stateMachine.ChangeState(enemyRobort0.attackState);
                    else stateMachine.ChangeState(enemyRobort0.attackState);

                }
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 7) 
            { 
            stateMachine.ChangeState(enemyRobort0.moveState);
                //Debug.Log("idle");
                //if (stateTimer < 0) Debug.Log("time");
            }
        }




    }
    public override void Exit()
    {
        base.Exit();
        enemyRobort0.ZeroVelocity();
    }

    private bool CanAttack()
    {
        if(Time.time >=enemy.lastTimeAttacked + enemy.attackCoolDown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }


}
