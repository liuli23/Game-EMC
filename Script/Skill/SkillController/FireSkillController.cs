using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class FireSkillController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CapsuleCollider2D cd;
    private Player player;
    private bool canRotate = true;

    [SerializeField] private float dis;
    [SerializeField] private float maxDis = 200;
    private float multiplier;
    [Header("������Ϣ")]
    [SerializeField]private float bounceSpeed;
    private bool isBouncing;
    private int bounceAmount;
    private List<Transform> enemyTarget;
    private int targetIndex;


    [Header("������Ϣ")]
    [SerializeField] private float pierceAmount;

    [Header("��ҩ��Ϣ")]
    [SerializeField] private bool isRegular = false;
    [SerializeField] private bool isJamming = false;
    [SerializeField] private float freezeTimeDuration;

    private ObjectPool<GameObject> _pool;

    // ͨ������������ö��������
    public void SetPool(ObjectPool<GameObject> pool)
    {
        _pool = pool;
    }

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CapsuleCollider2D>();
        player = PlayerManager.instance.player;
    }

    public void SetupBullet(Vector2 _direction,float _gravityScale,float _multiplier)
    {
        rb.linearVelocity = _direction;
        rb.gravityScale = _gravityScale;

        anim.SetBool("Fly", true);
        multiplier = _multiplier;
        //Invoke("DestoryMe", 5);//ʱ������

    }

    public void SetupBounce(bool _isBouncing, int _bounceAmount, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _bounceAmount;
        bounceSpeed = _bounceSpeed;

        enemyTarget = new List<Transform>();//����˵������ΪenemyTarget������Ϊ��˽�г�Ա��������Ҫ�Լ�����Ĭ�ϳ�ʼ��

    }    

    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void SetupDanyao(int _danyaoNo,float _freezeTimeDuration)//��ҩ����
    {
        if (_danyaoNo == 0) isRegular = true;
        if(_danyaoNo == 1) isJamming = true;

        freezeTimeDuration = _freezeTimeDuration;
    }

    private void ReleaseBullet()
    {
        if (gameObject.activeSelf && _pool != null)
        {
            _pool.Release(gameObject);
        }
    }

    private void DestoryMe()
    {
        dis = Vector3.Distance(rb.transform.position,player.transform.position);
        /*
        if(dis>maxDis)
            Destroy(gameObject);
        */
        if (dis > maxDis)
        {
            // �������ͷŻض����
            //FireSkill.bulletPool.Release(gameObject);
            // �Լ����ģ�Ϊ����֮ǰ����ϣ�����FireSkill�ű�����һ����̬��ObjectPool<GameObject>ʵ��������ͨ��������ʽ��ȡ
            // �������FireSkill��һ����̬����
            ReleaseBullet();
        }
    }//̫��ʱ���٣����Invoke��Ӧ�ÿ����Ż�


    public void ResetState()
    {
        // ��������״̬
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.gravityScale = 0;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = RigidbodyConstraints2D.None;

        // ������ײ��
        cd.enabled = true;

        // ���ñ任
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        transform.SetParent(null);

        // �����߼�״̬
        canRotate = true;
        pierceAmount = 0;
        bounceAmount = 0;
        enemyTarget?.Clear();
        CancelInvoke(); // ȡ�������ӳٵ���
    }



    private void Update()
    {
        if (canRotate)
            transform.right = rb.linearVelocity;

        BounceLogic();

        DestoryMe();//��������

    }


    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {


            while (enemyTarget.Count > 0 && !IsTargetValid(enemyTarget[targetIndex]))
            {
                enemyTarget.RemoveAt(targetIndex);
                if (enemyTarget.Count == 0)
                {
                    isBouncing = false;
                    ReleaseBullet();
                    return;
                }
                targetIndex = (targetIndex) % enemyTarget.Count;
            }


            //Debug.Log("Bouncing");
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                if (isRegular)
                {
                    player.stats.DoDamage(enemyTarget[targetIndex].GetComponent<CharacterStats>(), multiplier);
                    //�Ķ�˵����ʹ�ô���ֵ������˺������滻����Ч��
                    //enemyTarget[targetIndex].GetComponent<Enemy>().DamageEffect();
                }
                if (isJamming)
                {
                    player.stats.DoDamage(enemyTarget[targetIndex].GetComponent<CharacterStats>(), multiplier);
                    //�Ķ�˵����ʹ�ô���ֵ������˺������滻����Ч��
                    //enemyTarget[targetIndex].GetComponent<Enemy>().DamageEffect();
                    enemyTarget[targetIndex].GetComponent<Enemy>().StartCoroutine("FreezeTimerFor", freezeTimeDuration);
                }//�����Ż��������Ҿ�������һЩ

                targetIndex = (targetIndex + 1) % enemyTarget.Count;
                bounceAmount--;

                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    Invoke("ReleaseBullet", .1f); // �ӳ�1������
                }

            }

        }
    }


    private bool IsTargetValid(Transform target)
    {
        return target != null &&
               target.gameObject.activeInHierarchy &&
               target.GetComponent<Enemy>() != null;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isRegular)
        {
            //if (collision.GetComponent<Enemy>() == null) return;
            if (collision.GetComponent<Enemy>() != null)
                player.stats.DoDamage(collision.GetComponent<CharacterStats>(), multiplier);
            //�Ķ�˵����ʹ�ô���ֵ������˺������滻����Ч��
            //collision.GetComponent<Enemy>()?.DamageEffect();//���浯ҩ
        }

        if(isJamming)//���ŵ�
        {
            if(collision.GetComponent<Enemy>()!=null)
            {
                Enemy enemy = collision.GetComponent<Enemy>();
                player.stats.DoDamage(enemy.GetComponent<CharacterStats>(), multiplier);
                //�Ķ�˵����ʹ�ô���ֵ������˺������滻����Ч��
                //enemy.DamageEffect();
                enemy.StartCoroutine("FreezeTimerFor", freezeTimeDuration);//��������������ϳɸ��ŵ�����������

            }
        }

        SetupTargetForBounce(collision);

        //Debug.Log("Trigger Enter: " + collision.gameObject.name);
        StuckInto(collision);
    }

    private void SetupTargetForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        enemyTarget.Add(hit.transform);
                }
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            if (pierceAmount <= 0)
            {
                ReleaseBullet(); // ��͸�����þ������ͷ�
            }
            return;
        }

        cd.enabled = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 1)
            return;

        canRotate = false;
        anim.SetBool("Fly", false);
        transform.parent = collision.transform;

        // �ӳ��ͷ�
        Invoke("ReleaseBullet", 1f);
    }
}
