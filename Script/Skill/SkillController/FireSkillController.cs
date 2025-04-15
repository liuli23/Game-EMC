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
    [Header("弹射信息")]
    [SerializeField]private float bounceSpeed;
    private bool isBouncing;
    private int bounceAmount;
    private List<Transform> enemyTarget;
    private int targetIndex;


    [Header("穿刺信息")]
    [SerializeField] private float pierceAmount;

    [Header("弹药信息")]
    [SerializeField] private bool isRegular = false;
    [SerializeField] private bool isJamming = false;
    [SerializeField] private float freezeTimeDuration;

    private ObjectPool<GameObject> _pool;

    // 通过这个方法设置对象池引用
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
        //Invoke("DestoryMe", 5);//时间销毁

    }

    public void SetupBounce(bool _isBouncing, int _bounceAmount, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _bounceAmount;
        bounceSpeed = _bounceSpeed;

        enemyTarget = new List<Transform>();//设置说明，因为enemyTarget被设置为了私有成员，所以需要自己进行默认初始化

    }    

    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void SetupDanyao(int _danyaoNo,float _freezeTimeDuration)//弹药配置
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
            // 将对象释放回对象池
            //FireSkill.bulletPool.Release(gameObject);
            // 自己给的，为了与之前的配合，建议FireSkill脚本里有一个静态的ObjectPool<GameObject>实例，或者通过其他方式获取
            // 这里假设FireSkill有一个静态引用
            ReleaseBullet();
        }
    }//太久时销毁，结合Invoke，应该可以优化


    public void ResetState()
    {
        // 重置物理状态
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.gravityScale = 0;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = RigidbodyConstraints2D.None;

        // 重置碰撞体
        cd.enabled = true;

        // 重置变换
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        transform.SetParent(null);

        // 重置逻辑状态
        canRotate = true;
        pierceAmount = 0;
        bounceAmount = 0;
        enemyTarget?.Clear();
        CancelInvoke(); // 取消所有延迟调用
    }



    private void Update()
    {
        if (canRotate)
            transform.right = rb.linearVelocity;

        BounceLogic();

        DestoryMe();//距离销毁

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
                    //改动说明：使用带数值计算的伤害函数替换攻击效果
                    //enemyTarget[targetIndex].GetComponent<Enemy>().DamageEffect();
                }
                if (isJamming)
                {
                    player.stats.DoDamage(enemyTarget[targetIndex].GetComponent<CharacterStats>(), multiplier);
                    //改动说明：使用带数值计算的伤害函数替换攻击效果
                    //enemyTarget[targetIndex].GetComponent<Enemy>().DamageEffect();
                    enemyTarget[targetIndex].GetComponent<Enemy>().StartCoroutine("FreezeTimerFor", freezeTimeDuration);
                }//可以优化，但是我觉得清晰一些

                targetIndex = (targetIndex + 1) % enemyTarget.Count;
                bounceAmount--;

                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    Invoke("ReleaseBullet", .1f); // 延迟1秒销毁
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
            //改动说明：使用带数值计算的伤害函数替换攻击效果
            //collision.GetComponent<Enemy>()?.DamageEffect();//常规弹药
        }

        if(isJamming)//干扰弹
        {
            if(collision.GetComponent<Enemy>()!=null)
            {
                Enemy enemy = collision.GetComponent<Enemy>();
                player.stats.DoDamage(enemy.GetComponent<CharacterStats>(), multiplier);
                //改动说明：使用带数值计算的伤害函数替换攻击效果
                //enemy.DamageEffect();
                enemy.StartCoroutine("FreezeTimerFor", freezeTimeDuration);//上下两句可以整合成干扰弹攻击，随意

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
                ReleaseBullet(); // 穿透次数用尽立即释放
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

        // 延迟释放
        Invoke("ReleaseBullet", 1f);
    }
}
