using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class BlackholeSkillController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float startMoveSpeed;
    private float speedDecayRate;
    private float endMoveSpeed;
    private bool isMoving = true;

    private bool canGrow = true;
    private bool canShrink = false;
    private bool canJamming = false;

    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;

    private float attractForce = 5f; // �����ٶ�
    private float blackholeDuration = 5f; // �ڶ��ĳ���ʱ��
    private List<Transform> targets = new List<Transform>(); // ��������Ŀ���б�;
    private CharacterStats targetStats;
    private float multiplier;
    //private bool isAttracting = false;
    private float timer = 0f;

    public void SetupBlackhole(float _maxsize,float _growSpeed,float _shrinkSpeed,float _attractForce,bool _canJamming,float _blackholeDuration,
                               float _startMoveSpeed,float _speedDecayRate,float _endMoveSpeed,float _multiplier)
    {
        maxSize = _maxsize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        attractForce = _attractForce;
        blackholeDuration = _blackholeDuration;
        startMoveSpeed = _startMoveSpeed;
        speedDecayRate = _speedDecayRate;
        endMoveSpeed = _endMoveSpeed;
        multiplier = _multiplier;
        canJamming= _canJamming;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }


    private void ApplyInitialForce()
    {
        if (rb != null)
        {
            Vector2 direction = new Vector2(PlayerManager.instance.player.facingDirection,0).normalized;

            rb.linearVelocity = direction * startMoveSpeed;
            Debug.Log(rb.linearVelocity);
        }
    }

    private void Start()
    {
        ApplyInitialForce();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        //PlayerManager.instance.player.fx.ScreenShake(0.2f, Random.Range(1, 1.5f), Random.Range(1, 1.5f), 0,false);

        ShrinkAndDestory();


    }

    private void DecreaseMovement()
    {
        if (!isMoving || rb == null) return;

        // �ٶ�˥��
        rb.linearVelocity *= speedDecayRate;

        // ���ٶȵ�����Сֵʱֹͣ�ƶ�
        if (rb.linearVelocity.magnitude < endMoveSpeed)
        {
            rb.linearVelocity = Vector2.zero;
            isMoving = false;
        }
    }

    private void ShrinkAndDestory()
    {
        if (timer >= blackholeDuration)
        {
            canGrow = false;
            canShrink = true;
        }
        if (canShrink)
        {
            if (gameObject != null)
            {
                transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
                if (transform.localScale.x < 0)
                { 
                    canShrink = false; 
                    Destroy(gameObject); 
                }

            }

        }
    }

    private void FixedUpdate()
    {
        AttractEnemies();
        DecreaseMovement();
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            targets.Add(collision.transform);
            collision.GetComponent<Enemy>().FreezeTime(true);


        }

    }
    */


    private void OnDestroy()
    {
        // �ⶳ���е���
        foreach (Transform target in targets)
        {
            if (target.GetComponent<Enemy>() != null)
            {
                target.GetComponent<Enemy>().FreezeTime(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !targets.Contains(collision.transform))
        if(collision.CompareTag("Enemy"))
        {
            targets.Add(collision.transform);
            if (SkillManager.instance.blackhole.canJamming)
            {
                if(canJamming)collision.GetComponent<Enemy>().FreezeTime(true);

                targetStats = collision.GetComponent<CharacterStats>();
                //Debug.Log(targetStats == null);
                if (targetStats != null)
                {
                        //Debug.Log(" ��ʼ�����Ե��� DoDamage ����");
                        if (canJamming)
                            InvokeRepeating(nameof(DealDamage), .2f, .5f);
                        else if (!canJamming)
                            InvokeRepeating(nameof(DealDamage), .2f, .3f);
                }

                //Debug.Log("ֱ�ӵ���");
                //PlayerManager.instance.player.stats.DoDamage(collision.GetComponent<CharacterStats>(),.3f);

            }
        }
    }//�Ķ�˵����������˶�ν��봥������Χ�����ܻᱻ�ظ���ӵ� targets �б��С�����ͨ������Ƿ��Ѿ��������б����������ظ���ӡ�

    private void OnTriggerExit2D(Collider2D collision)
    {
        // ֹͣ�����Ե���
        CancelInvoke(nameof(DealDamage));
        targets.Remove(collision.transform);

    }

    private void DealDamage()
    {
        // ���� DoDamage ����
        if (targetStats != null)
        {
            PlayerManager.instance.player.stats.DoDamage(targetStats,multiplier);
        }
    }


    /*
    private void AttractEnemies()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, maxSize);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector3 direction = transform.position - collider.transform.position;
                    rb.linearVelocity = direction.normalized * attractSpeed;
                }
            }
        }
    }

    */


    private void AttractEnemies()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, maxSize);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 direction = transform.position - collider.transform.position;
                    float distance = direction.magnitude;
                    float force = attractForce * (1 - distance / maxSize); // ���ݾ������������
                    
                    //rb.linearVelocity = direction.normalized * force * Time.deltaTime;
                    rb.AddForce(direction.normalized * force, ForceMode2D.Force);//�Ķ�˵�����ŵ�FixUpdate����
                    //�Ķ�˵�����ĳ�AddForce����Ч������һ��
                    //Debug.Log($"Applying force: {force} to enemy {collider.name}");
                }
            }
        }
    }//�Ķ�˵����
     //֡�ʶ����� ��ǰ�����У������ٶ�û�п���֡�ʣ����ܻᵼ�²�ͬ֡���µı��ֲ�һ�¡�����ͨ������ Time.deltaTime ��ȷ��֡�ʶ�����
     //���ݾ�������������� ����ڶ�Խ���ĵ��ˣ����������Ը�ǿ����������������ֱ����

    /*
    private IEnumerator AttractEnemiesCoroutine()
    {
        while (true)
        {
            AttractEnemies();
            yield return null;
        }
    }
    */
    



    
    /*
    private IEnumerator AttractEnemiesCoroutine()
    {

        timer = 0f; // ��ʼ����ʱ��

        while (timer < blackholeDuration && gameObject != null)//�Ķ�˵������Э���м��ڶ��Ƿ����٣� ��Э�̵�ÿ�ε����У����ڶ��Ƿ���Ȼ���ڡ�����ڶ������٣�Э��Ӧ����ǰ�˳���
        {
            Debug.Log("����");
            AttractEnemies();
            timer += Time.deltaTime;
            yield return null;
        }

        // �ڶ�����ʱ����������ٺڶ�
        //Destroy(gameObject);
        // �ڶ�����ʱ����������ٺڶ�
        if (gameObject != null)
        {
            Debug.Log("Blackhole duration ended. Destroying blackhole.");
            Destroy(gameObject);
        }
    }
    */
    
    //�Ķ�˵�������������߼�
    //�Ķ�˵��2������Э����
    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxSize*0.5f);
    }

}

