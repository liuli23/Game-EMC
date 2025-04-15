using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player _player, PlayerStateMach _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        PreUpdate();

        base.Update();

        if (!player.IsGrounding())
        {
            stateMachine.ChangeState(player.airState);
        }


        //if (Input.GetKeyDown(KeyCode.O) && SkillManager.instance.blackhole.CanUseSkill())
        if (player.inputControl.Player.Blackhole.WasPressedThisFrame() && SkillManager.instance.blackhole.CanUseSkill())
            stateMachine.ChangeState(player.blackholeState);//改动说明：加入技能逻辑

        if (player.inputControl.Player.NormalFire.IsPressed())
        {
            player.fireWay = 1;
            stateMachine.ChangeState(player.aimState);
        }


        if (player.inputControl.Player.SpecialFire.IsPressed() && SkillManager.instance.fire.CanUseSkill() && SkillManager.instance.fire.canAim)
        {
            player.fireWay = 2;
            stateMachine.ChangeState(player.aimState);//改动说明：加入技能逻辑
        }

        if(player.inputControl.Player.NormalAttack.IsPressed() && player.IsGrounding())
        {
            stateMachine.ChangeState(player.normalAttackState);
        }

        if(player.inputControl.Player.Conter.WasPressedThisFrame() && player.IsGrounding())
        {
            stateMachine.ChangeState(player.counterAttackState);
        }


        if (player.inputControl.Player.Jump.WasPressedThisFrame() && player.IsGrounding())
        { 
            stateMachine.ChangeState(player.jumpState);
        }
    }


    protected virtual void PreUpdate()
    {
    }


}
