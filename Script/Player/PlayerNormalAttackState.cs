using UnityEngine;

public class PlayerNormalAttackState : PlayerState
{
    private int comboCounter;

    private float lastTimeAttacked;
    //private float comboWindow = 2;

    public PlayerNormalAttackState(Player _player, PlayerStateMach _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //if(Random.Range(20,100)>50)
            AudioManager.instance.PlaySFX(12,null,true, 1.3f , 2.1f);//攻击音效
        //else AudioManager.instance.PlaySFX(9);

        //xInput = 0;可能有bug

        stateTimer = .1f;

        if (Time.time >= lastTimeAttacked + player.comboWindow)
            comboCounter = 0;

        float attackDirection = player.facingDirection;
        if(xInput!=0)attackDirection = xInput;


        player.anim.SetInteger("ComboCounter", comboCounter);

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDirection, player.attackMovement[comboCounter].y);

        //player.anim.speed = 1.2f;//设置动画速度所有动画生效，所以进入时设置，只设置攻击速度


        player.skill.clone.CreateCloneOnNormalAttack();
    }

    public override void Exit()
    {
        base.Exit();

        comboCounter = (comboCounter + 1)%2;
        lastTimeAttacked = Time.time;

        player.StartCoroutine("BusyFor", .1f);

        player.ZeroVelocity();
        //player.anim.speed = 1;

    }

    public override void Update()
    {
        base.Update();
        /*
        if (triggerCalled && Input.GetKey(KeyCode.J))
        {
            //Debug.Log("counter++");
            comboCounter = (comboCounter + 1) % 2;
            triggerCalled = false;
            player.anim.SetInteger("ComboCounter", comboCounter);
        }

        else if (triggerCalled && !Input.GetKey(KeyCode.J))
            stateMachine.ChangeState(player.idelState);
        */
        if (triggerCalled)
            stateMachine.ChangeState(player.idelState);

        if (stateTimer < 0) 
        {
        rb.linearVelocity = new Vector2((float)(xInput * player.moveSpeed * 0.1), 0);
        }
    }
}
