using UnityEngine;
public class EnemyState 
{

    protected EnemyStateMachine stateMachine;
    protected Enemy enemy;
    protected Rigidbody2D rb;

    protected float stateTimer;
    protected bool triggerCalled;
    private string animBoolName;

    public EnemyState(Enemy _enemy,EnemyStateMachine _stateMachine,string _animBoolName)
    {
        enemy = _enemy;
        stateMachine = _stateMachine;
        animBoolName = _animBoolName;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        enemy.anim.SetBool(animBoolName, true);
        rb = enemy.rb;

    }

    public virtual void Exit() 
    {
        enemy.anim.SetBool(animBoolName, false);
        enemy.AssignLastAnimName(animBoolName);

    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
        //Debug.Log("false");
        //Debug.Log("´¥·¢Æ÷" + triggerCalled);
    }

}

