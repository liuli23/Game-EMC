using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{

    private bool canCreateClone;
    public PlayerCounterAttackState(Player _player, PlayerStateMach _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();



        canCreateClone = true;
        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("CounterAttackSuccess", false);

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.ZeroVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStanded())
                {
                    stateTimer = 10;//持续久一点，不重要；
                    player.anim.SetBool("CounterAttackSuccess", true);
                    {
                        AudioManager.instance.PlaySFX(25, null);
                        //Debug.Log("震屏");
                        player.fx.ScreenShake();

                        TimeScaleManager.instance.BulletTime(0.2f);
                        //canCreateClone = false;
                        if (canCreateClone)
                        {
                            player.skill.clone.CreateCloneOnCounterAttack(hit.transform);
                        }
                    }
                }
            }
        }

        if (stateTimer < 0 || triggerCalled)
        {
            stateMachine.ChangeState(player.idelState);
        }

    }
}
