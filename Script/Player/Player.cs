using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : Entity
{
    public InputControl inputControl;
    public InputAction normalAttack;



    [Header("攻击")]
    public Vector2[] attackMovement;
    public float comboWindow;
    public float counterAttackDuration = .2f;
    public int fireWay = 0;
    public float xiaPiSpeed;
    public GameObject xiaPiFanWei;


    [Header("移动")]
    public float moveSpeed = 8f;
    public float jumpForce;
    //public float xInput;

    public bool canDash = true;
    public bool isBusy {  get; private set; }

    [Header("冲刺")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDirection;
    [SerializeField] private float dashCoolDown;
    //private float dashTimer;



    public SkillManager skill {  get; private set; }



    //#region States
    public PlayerStateMach stateMachine { get; private set; }
    public PlayerIdelState idelState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerDashState dashState { get; private set; }  
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }  
    public PlayerNormalAttackState normalAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerAimState aimState { get; private set; }
    public PlayerBlackholeState blackholeState { get; private set; }
    public PlayerDeathState deathState { get; private set; }
    public PlayerXiaPiState xiaPiState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        inputControl = new InputControl();




        stateMachine = new PlayerStateMach();

        idelState = new PlayerIdelState(this, stateMachine, "Idel");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        normalAttackState = new PlayerNormalAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        aimState = new PlayerAimState(this, stateMachine, "Aim");
        blackholeState = new PlayerBlackholeState(this, stateMachine, "Blackhole");
        deathState = new PlayerDeathState(this, stateMachine, "Death");
        xiaPiState = new PlayerXiaPiState(this, stateMachine, "XiaPi");
    }


    
    private void OnEnable()
    {
        inputControl.Enable();

        //normalAttack.performed += 


    }

    private void OnDisable()
    {
        inputControl.Disable();
    }
    






    protected override void Start()
    {
        base.Start();

        skill = SkillManager.instance;

        stateMachine.Initialize(idelState);
    }

    protected override void Update()
    {
        //inputDirection = inputActions.



        if (Time.timeScale == 0 || stats.isDead) return;

        base .Update();

        stateMachine.currentState.Update();
        //FlipControl(Input.GetAxisRaw("Horizontal"));

        FlipControl(inputControl.Player.Move.ReadValue<Vector2>().x);//新输入系统

        //FlipControl(rb.linearVelocity.x);
        CheckForDashInput();



        //改动说明：此处使得任意地面状态可以进入弹反状态，即可打断攻击状态和冲刺状态，若想保留攻击后腰，注释掉此部分代码
        //if (Input.GetKeyDown(KeyCode.I) && IsGrounding())
        if (inputControl.Player.Conter.WasPressedThisFrame() && IsGrounding())
        {
            stateMachine.ChangeState(counterAttackState);
        }


        if(inputControl.Player.Crystal.WasPressedThisFrame() &&skill.crystal.CanUseSkill() && skill.crystal.canUseCrystal)
        {
            skill.crystal.UseSkill();
        }


        if(inputControl.Player.Flask.WasPressedThisFrame())
            Inventory.instance.UseFlask();


        //if(Input.GetKeyDown(KeyCode.J)&&!IsGrounding())
        if (inputControl.Player.NormalAttack.WasPressedThisFrame() && !IsGrounding())
        {
            {
                stateMachine.ChangeState(xiaPiState);
            }
        }

        /*
        if (Input.GetKeyDown(KeyCode.Alpha1)) // 按下空格键启动卡肉效果的子弹时间
        {
            TimeScaleManager.instance.BulletTime();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) // 按下左Shift键启动带有渐变减速的子弹时间
        {
            TimeScaleManager.instance.BulletTimeWithSlowDown();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) // 按下E键手动恢复时间
        {
            TimeScaleManager.instance.RestoreNormalTime();
        }

        */


        Debug.Log(IsWalling());
    }


    //协程控制

    #region
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true; ;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    #endregion


    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    
    /*
    public void NormalAttacking()
    {
        if(Input.GetKey(KeyCode.J))
        {
            stateMachine.currentState.AnimationNotFinshTrigger();
            Debug.Log("Attacking");
        }
    }
    */


    private void CheckForDashInput()
    {

        //dashTimer -= Time.deltaTime;
        //if (Input.GetKeyDown(KeyCode.L) && dashTimer < 0 )//改动：改用技能管理器

        //if (Input.GetKeyDown(KeyCode.L) && SkillManager.instance.dash.CanUseSkill())
        if (inputControl.Player.Dash.WasPressedThisFrame() && SkillManager.instance.dash.CanUseSkill())
        {

            //dashTimer = dashCoolDown;改动：改用技能管理器

            //dashDirection = Input.GetAxisRaw("Horizontal");
            //dashDirection = inputControl.Player.Move.ReadValue<Vector2>().x;
            //
            //if (dashDirection == 0)
            //    dashDirection = facingDirection;

            stateMachine.ChangeState(dashState);

        }
        if(IsGrounding()) xiaPiFanWei.SetActive(false);//加一层保障




    }







    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deathState);   
    }










}
