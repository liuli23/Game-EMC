using DG.Tweening;
using UnityEngine;

public class PlayerXiaPiState : PlayerState
{
    public PlayerXiaPiState(Player _player, PlayerStateMach _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        //if(player.xiaPiFanWei.activeSelf==false) 
        //    player.xiaPiFanWei.SetActive(true);
        //else 
        //    player.xiaPiFanWei.SetActive(false);
        //Debug.Log("¶¯»­");
    }

    public override void Enter()
    {
        
        base.Enter();
        AudioManager.instance.PlaySFX(12, null, true, 1.3f, 2.1f);//¹¥»÷ÒôÐ§
    }

    public override void Exit()
    {
        if(player.xiaPiFanWei.activeSelf)
            player.xiaPiFanWei.SetActive(false);
        base.Exit();

    }

    public override void Update()
    {
        base.Update();

        if (xInput != 0)
            player.SetVelocity(player.moveSpeed * .8f * xInput, rb.linearVelocity.y);

        if (player.IsWalling()) player.stateMachine.ChangeState(player.wallSlideState);
        if (player.IsGrounding()) player.stateMachine.ChangeState(player.idelState);


        if(triggerCalled) player.stateMachine.ChangeState(player.airState);

    }


    public void XiaPiFanWei()
    {

        if (player.xiaPiFanWei.activeSelf == false)
            player.xiaPiFanWei.SetActive(true);
        else
            player.xiaPiFanWei.SetActive(false);

    }


}
