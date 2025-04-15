using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{

    //���
    #region 
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D cd { get; private set; }

    #endregion


    [Header("����")]
    [SerializeField] protected Vector2 knockbackPower;
    [SerializeField] protected float knockbackTime;
    protected bool isknockbacked;
    public int knockbackDirection {  get; private set; }




    [Header("��ײ���")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask groundMask;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;

    public int facingDirection = 1;
    //private bool facingRight = true;
    public bool canFlip = true;


    public System.Action onFlipped;


    protected virtual void Awake()
    {
        fx = GetComponent<EntityFX>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        stats = GetComponentInChildren<CharacterStats>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Start()
    {


    }

    protected virtual void Update()
    {

    }

    public virtual void DamageImpact() //=> StartCoroutine("HitKnockback");
    {
        StartCoroutine("HitKnockback");//�Ķ�˵�����ƻ���������CharacterStats��Э�̻ᵼ�������쳣
        fx.StartCoroutine("FlashFX");//�Ķ�˵��������CharacterStats
        //Debug.Log(gameObject.name + "������");

    }
    
    public virtual void SetupKnockbackDir(Transform _damageDirection)
    {
        if (_damageDirection.position.x > transform.position.x)
            knockbackDirection = -1;
        else if(_damageDirection.position.x < transform.position.x)
            knockbackDirection = 1;
    }


    protected virtual IEnumerator HitKnockback()
    {
        isknockbacked = true; ;
        rb.linearVelocity = new Vector2(knockbackPower.x * knockbackDirection, knockbackPower.y);
        //Debug.Log("����");
        yield return new WaitForSeconds(knockbackTime);
        isknockbacked = false;
         
    }

    //��ײ���
    #region
    public virtual bool IsGrounding() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundMask);
    public virtual bool IsWalling() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, groundMask);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion


    //�������
    #region
    public virtual void Flip()
    {

        facingDirection = facingDirection * -1;
        //facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        //onFlipped();//���ڿ���������ᱨ��
        onFlipped?.Invoke();
    }

    public virtual void FlipControl(float _x)
    {
        if (_x * facingDirection < 0 && canFlip)//&& dashTimer < dashCoolDown-dashDuration)
            Flip();
    }

    #endregion


    //�����ٶ�
    #region
    public virtual void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isknockbacked) return;
        rb.linearVelocity = new Vector2(_xVelocity, _yVelocity);
        FlipControl(_xVelocity);//ת�������������
    }

    public virtual void ZeroVelocity()
    {
        if (isknockbacked) return;
        rb.linearVelocity = Vector2.zero;
    }
    #endregion


    
    /*
    public void MakeTransprent(bool _transprent)
    {
        if (_transprent)
            sr.color = Color.clear;
        else 
            sr.color = Color.white;
    }
    */
    //�Ķ�˵��������EntityFX


    public virtual void Die()
    {

    }

}
