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

    private float attractForce = 5f; // 吸附速度
    private float blackholeDuration = 5f; // 黑洞的持续时间
    private List<Transform> targets = new List<Transform>(); // 被吸附的目标列表;
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

        // 速度衰减
        rb.linearVelocity *= speedDecayRate;

        // 当速度低于最小值时停止移动
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
        // 解冻所有敌人
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
                        //Debug.Log(" 开始周期性调用 DoDamage 方法");
                        if (canJamming)
                            InvokeRepeating(nameof(DealDamage), .2f, .5f);
                        else if (!canJamming)
                            InvokeRepeating(nameof(DealDamage), .2f, .3f);
                }

                //Debug.Log("直接调用");
                //PlayerManager.instance.player.stats.DoDamage(collision.GetComponent<CharacterStats>(),.3f);

            }
        }
    }//改动说明：如果敌人多次进入触发器范围，可能会被重复添加到 targets 列表中。可以通过检查是否已经存在于列表中来避免重复添加。

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 停止周期性调用
        CancelInvoke(nameof(DealDamage));
        targets.Remove(collision.transform);

    }

    private void DealDamage()
    {
        // 调用 DoDamage 方法
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
                    float force = attractForce * (1 - distance / maxSize); // 根据距离调整吸附力
                    
                    //rb.linearVelocity = direction.normalized * force * Time.deltaTime;
                    rb.AddForce(direction.normalized * force, ForceMode2D.Force);//改动说明，放到FixUpdate里了
                    //改动说明，改成AddForce可能效果更好一点
                    //Debug.Log($"Applying force: {force} to enemy {collider.name}");
                }
            }
        }
    }//改动说明：
     //帧率独立： 当前代码中，吸附速度没有考虑帧率，可能会导致不同帧率下的表现不一致。可以通过乘以 Time.deltaTime 来确保帧率独立。
     //根据距离调整吸附力： 距离黑洞越近的敌人，吸附力可以更强，这样更符合物理直觉。

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

        timer = 0f; // 初始化计时器

        while (timer < blackholeDuration && gameObject != null)//改动说明：在协程中检查黑洞是否被销毁： 在协程的每次迭代中，检查黑洞是否仍然存在。如果黑洞被销毁，协程应该提前退出。
        {
            Debug.Log("吸附");
            AttractEnemies();
            timer += Time.deltaTime;
            yield return null;
        }

        // 黑洞持续时间结束，销毁黑洞
        //Destroy(gameObject);
        // 黑洞持续时间结束，销毁黑洞
        if (gameObject != null)
        {
            Debug.Log("Blackhole duration ended. Destroying blackhole.");
            Destroy(gameObject);
        }
    }
    */
    
    //改动说明：增加销毁逻辑
    //改动说明2：弃用协程了
    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxSize*0.5f);
    }

}

