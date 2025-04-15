using UnityEngine;

public class PlayerIdelState : PlayerGroundState
{
    public PlayerIdelState(Player _player, PlayerStateMach _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.ZeroVelocity();
        player.canFlip = false;

    }

    public override void Exit()
    {
        base.Exit();
        player.canFlip = true;
    }

    public override void Update()
    {

        base.Update();
        /*
        if(xInput==player.facingDirection && player.IsWalling())
        {
            return;
        }
        */


        //player.ZeroVelocity();


        if (xInput != 0 && !player.isBusy)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }
 







    protected override void PreUpdate()
    {
        player.ZeroVelocity();
    }


}
