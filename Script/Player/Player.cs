using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : Entity
{
    public InputControl inputControl;
    public InputAction normalAttack;



    [Header("����")]
    public Vector2[] attackMovement;
    public float comboWindow;
    public float counterAttackDuration = .2f;
    public int fireWay = 0;
    public float xiaPiSpeed;
    public GameObject xiaPiFanWei;


    [Header("�ƶ�")]
    public float moveSpeed = 8f;
    public float jumpForce;
    //public float xInput;

    public bool canDash = true;
    public bool isBusy {  get; private set; }

    [Header("���")]
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

        FlipControl(inputControl.Player.Move.ReadValue<Vector2>().x);//������ϵͳ

        //FlipControl(rb.linearVelocity.x);
        CheckForDashInput();



        //�Ķ�˵�����˴�ʹ���������״̬���Խ��뵯��״̬�����ɴ�Ϲ���״̬�ͳ��״̬�����뱣������������ע�͵��˲��ִ���
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
        if (Input.GetKeyDown(KeyCode.Alpha1)) // ���¿ո����������Ч�����ӵ�ʱ��
        {
            TimeScaleManager.instance.BulletTime();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) // ������Shift���������н�����ٵ��ӵ�ʱ��
        {
            TimeScaleManager.instance.BulletTimeWithSlowDown();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) // ����E���ֶ��ָ�ʱ��
        {
            TimeScaleManager.instance.RestoreNormalTime();
        }

        */


        Debug.Log(IsWalling());
    }


    //Э�̿���

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
        //if (Input.GetKeyDown(KeyCode.L) && dashTimer < 0 )//�Ķ������ü��ܹ�����

        //if (Input.GetKeyDown(KeyCode.L) && SkillManager.instance.dash.CanUseSkill())
        if (inputControl.Player.Dash.WasPressedThisFrame() && SkillManager.instance.dash.CanUseSkill())
        {

            //dashTimer = dashCoolDown;�Ķ������ü��ܹ�����

            //dashDirection = Input.GetAxisRaw("Horizontal");
            //dashDirection = inputControl.Player.Move.ReadValue<Vector2>().x;
            //
            //if (dashDirection == 0)
            //    dashDirection = facingDirection;

            stateMachine.ChangeState(dashState);

        }
        if(IsGrounding()) xiaPiFanWei.SetActive(false);//��һ�㱣��




    }







    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deathState);   
    }










}
