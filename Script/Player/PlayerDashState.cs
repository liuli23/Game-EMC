using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashState : PlayerState
{



    public PlayerDashState(Player _player, PlayerStateMach _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.instance.PlaySFX(24, null);
        //SkillManager.instance.clone.CreateClone(player.transform);
        //改动：在玩家脚本中创建了SkillManager的引用
        player.skill.clone.CreateCloneOnDashStart();

        stateTimer = player.dashDuration;
        //player.canFlip = false;
        player.stats.MakeWuDi(true);

        //if(xInput!=0)player.dashDirection = xInput;
        player.dashDirection = player.inputControl.Player.Move.ReadValue<Vector2>().x;

        if (player.dashDirection == 0)
            player.dashDirection = player.facingDirection;
    }

    public override void Exit()
    {
        player.SetVelocity(0, rb.linearVelocity.y);
        //player.canFlip = true;
        player.skill.clone.CreateCloneOnDashOver();
        base.Exit();
        player.stats.MakeWuDi(false);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(player.dashSpeed * player.dashDirection, 0);

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.idelState);
        }

        player.fx.CreateAfterImage();
    }
}
