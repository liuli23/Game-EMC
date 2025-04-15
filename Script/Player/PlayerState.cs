using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState 
{
    protected PlayerStateMach stateMachine;
    protected Player player;

    protected Rigidbody2D rb;

    public float xInput;
    public float yInput;
    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;


    public PlayerState(Player _player, PlayerStateMach _stateMachine ,string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        triggerCalled = false;

    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        xInput = PlayerManager.instance.player.inputControl.Player.Move.ReadValue<Vector2>().x;
        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("ySpeed", rb.linearVelocity.y);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
        //Debug.Log("true");
    }
    /*
    public virtual void AnimationNotFinshTrigger()
    {
        triggerCalled = false;
    }
    */

}
