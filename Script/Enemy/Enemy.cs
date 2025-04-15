using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public enum CheckWay
{
    line,
    cycle,
};

public class Enemy : Entity
{
    [SerializeField] protected LayerMask PlayerMask;
    [SerializeField] public float deathSpeed;

    [Header("眩晕信息")]
    public float standDuration;
    public Vector2 standDirection;
    protected bool canBeStanded;
    [SerializeField] protected bool standInformation;
    [SerializeField] protected GameObject counterImage;


    [Header("移动信息")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;
    public float defaultMoveSpeed;


    [Header("攻击信息")]
    public float attackDistance;
    public float attackCoolDown;
    public CheckWay checkWay = CheckWay.line;
    [HideInInspector]public float lastTimeAttacked;


    [Header("技能信息")]
    public bool canUseSkill = false;
    public float skillCoolDown;
    public float lastTimeSkill;


    public EnemyStateMachine stateMachine { get; private set; }
    public string lastAnimBoolName {  get; private set; }

    public GameObject[] fanwei;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();

        defaultMoveSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();



        //if(!isknockbacked)FlipControl(rb.linearVelocity.x);//转向问题改这里和SetVelocity


        //Debug.Log(IsPlayering().collider.gameObject.name + "I See");
    }

    public virtual void AssignLastAnimName(string _animBoolName)
    {
        lastAnimBoolName = _animBoolName;
    }




    public virtual void FreezeTime(bool _timeFrozen)
    {
        if(_timeFrozen)
        {
            moveSpeed = 0f;
            anim.speed = 0f;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;    
        }
    }


    protected virtual IEnumerator FreezeTimerFor(float _second)
    {
        FreezeTime(true);

        yield return new WaitForSeconds(_second);

        FreezeTime(false);
    }    

    //格挡反击
    #region
    public virtual void OpenCounterAttackWindow()
    {
        canBeStanded = standInformation;
        counterImage.SetActive(true);

    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStanded = false;
        counterImage.SetActive(false);

    }

    #endregion


    public virtual bool CanBeStanded()
    {
        if(canBeStanded)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }



    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();//动画结束，通过动画事件调用


    public virtual RaycastHit2D IsPlayering() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, 50, PlayerMask);



    
   


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDirection, transform.position.y));
    }



    public void OpenFanwei(int xfanwei)
    {
        int length = fanwei.Length;
        if (xfanwei < length)
        {
            if (fanwei[xfanwei].activeSelf == false)
            {
                fanwei[xfanwei].SetActive(true);
            }
            else fanwei[xfanwei].SetActive(false);
        }
    }




    public bool Boss1CanUseSkill()
    {
        if (Time.time > lastTimeSkill + skillCoolDown) return true;
        else return false;
    }

}
