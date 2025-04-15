using UnityEngine;

public class PlayerBlackholeState : PlayerGroundState
{
    public PlayerBlackholeState(Player _player, PlayerStateMach _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
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
        base.Update();

        player.ZeroVelocity();
        if (Input.GetKeyUp(KeyCode.O))
        {
            stateMachine.ChangeState(player.idelState);
        }
        /*
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (player.transform.position.x > mousePosition.x && player.facingDirection == 1)
            player.Flip();
        else if (player.transform.position.x < mousePosition.x && player.facingDirection == -1)
            player.Flip();
        */
    }
}
