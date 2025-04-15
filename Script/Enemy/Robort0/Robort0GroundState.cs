using UnityEngine;

public class Robort0GroundState : EnemyState
{
    protected EnemyRobort0 enemyRobort0;
    protected Transform player;

    public Robort0GroundState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemyRobort0 = enemy as EnemyRobort0;

        //player = GameObject.Find("Player").transform;//改用玩家管理器
        player = PlayerManager.instance.player.transform;


    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(enemyRobort0.IsPlayering() || Vector2.Distance( enemyRobort0.transform.position , player.position ) < 2)
        {
            stateMachine.ChangeState(enemyRobort0.battleState);
            //Debug.Log("Battle");
        }

    }
}
