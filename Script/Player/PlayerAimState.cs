using UnityEngine;

public class PlayerAimState : PlayerState
{
    public PlayerAimState(Player _player, PlayerStateMach _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //player.ZeroVelocity();

        if (player.fireWay == 2)
        {
            TimeScaleManager.instance.BulletTimeWithSlowDown();
            player.skill.fire.DotsActive(true);
            player.canFlip = false;
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.canFlip = true;
    }

    public override void Update()
    {
        base.Update();


        player.ZeroVelocity();
        if (player.inputControl.Player.SpecialFire.WasReleasedThisFrame())
        {
            TimeScaleManager.instance.RestoreNormalTime();
            stateMachine.ChangeState(player.idelState);
            player.fx.ScreenShake(0.2f, -2, 1, 0);
        }

        if(player.inputControl.Player.NormalFire.WasReleasedThisFrame())
        {
            stateMachine.ChangeState(player.idelState);
            player.fx.ScreenShake(0.2f, -2, 1, 0);
        }

        if (player.fireWay == 2)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (player.transform.position.x > mousePosition.x && player.facingDirection == 1)
                player.Flip();
            else if (player.transform.position.x < mousePosition.x && player.facingDirection == -1)
                player.Flip();
        }

    }
}
