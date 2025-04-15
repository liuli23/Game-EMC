using UnityEngine;
using Pathfinding;

public enum CrystalState
{
    Launching,
    Rotating,
    Homing
}

public class CrystalSkillController : MonoBehaviour
{
    private Animator anim;
    private CircleCollider2D cd;
    private Rigidbody2D rb;

    //新建
    private Seeker seeker;
    private Path path;
    private int currentWaypoint;
    // 状态控制
    private CrystalState currentState = CrystalState.Launching;
    // 运动参数
    [Header("Launch Phase")]
    [SerializeField] private float launchSpeed = 10f;
    [SerializeField] private float launchDistance = 3f;
    private Vector2 launchDirection;

    [Header("Rotation Phase")]
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float rotationDuration = 1f;
    [SerializeField]private float rotationAngle;


    [SerializeField] private Transform[] rotationCenters; // 通过Inspector设置的圆心位置
    [SerializeField] private float rotationRadius = 1f;  // 圆周运动半径

    private Vector2 currentRotationCenter; // 当前随机选择的圆心
    private float currentRotationAngle;    // 当前旋转角度


    private float rotationTimer;

    [Header("Homing Phase")]
    [SerializeField] private float homingSpeed = 5f;
    [SerializeField] private float nextWaypointDistance = 1f;
    [SerializeField] private LayerMask obstacleMask;

    ///////////////////////////////////////////////////////////////
    
    private Transform closestTarget;
    //private bool canMoveToEnemy;
    private bool canExplode;
    private bool canGrow =false;

    //private float moveSpeed;
    private float crystalExistTimer;
    private float growSpeed;
    private float multiplier;


    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        cd = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();



    }

    private void Start()
    {
        // 查找场景中所有带标签的物体
        GameObject[] centers = GameObject.FindGameObjectsWithTag("RotationCenter");
        rotationCenters = new Transform[centers.Length];

        for (int i = 0; i < centers.Length; i++)
        {
            rotationCenters[i] = centers[i].transform;
        }

        // 验证至少有三个圆心
        if (rotationCenters.Length < 3)
            Debug.LogError("场景中需要至少三个带 RotationCenter 标签的物体");
    }



public void SetupCrystal(float _crystalExist,bool _canExplode,bool _canMoveToEnemy,float _launchSpeed,float _homingSpeed,float _growSpeed,Transform _closestTarget,float _multiplier)
    {
        crystalExistTimer = _crystalExist;
        //canMoveToEnemy = _canMoveToEnemy;
        launchSpeed = _launchSpeed;
        canExplode = _canExplode;
        growSpeed = _growSpeed;
        closestTarget = _closestTarget;
        homingSpeed = _homingSpeed;
        multiplier = _multiplier;
        // 初始化发射阶段
        InitLaunchPhase();


    }
    
    private void InitLaunchPhase()//初始化发射状态以及发射角度
    {
        /*
        // 计算45度斜后方发射方向（假设玩家面朝右侧）
        launchDirection = (new Vector2(-PlayerManager.instance.player.facingDirection,0) + Vector2.up).normalized;
        rb.linearVelocity = launchDirection * launchSpeed;
        currentState = CrystalState.Launching;
        */


        // 随机生成发射角度（角色后方到上方90度范围）
        float minAngle = 135f; // 正后方（180度）到正上方（90度）之间的范围
        float maxAngle = 90f;
        float randomAngle = Random.Range(minAngle, maxAngle);

        // 根据玩家朝向调整角度方向
        if (PlayerManager.instance.player.facingDirection < 0)
            randomAngle = 180f - randomAngle;

        launchDirection = Quaternion.Euler(0, 0, randomAngle) * Vector2.right;
        rb.linearVelocity = launchDirection * launchSpeed;
        currentState = CrystalState.Launching;
        //改动说明：范围内某个随机角度

    }



    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;
        transform.right = rb.linearVelocity;

        if (crystalExistTimer < 0) FinishCrystal();
        /*
        if (canMoveToEnemy)
        {
            transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, closestTarget.position) < 1)
                FinishCrystal();
        
        }
        */
        if(canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
        }

        switch (currentState)
        {
            case CrystalState.Launching:
                UpdateLaunchPhase();
                break;
            case CrystalState.Rotating:
                UpdateRotationPhase();
                break;
            case CrystalState.Homing:
                UpdateHomingPhase();
                break;
        }

    }

    #region Launch Phase
    
    private void UpdateLaunchPhase()
    {
        if (Vector2.Distance(transform.position, PlayerManager.instance.player.transform.position) >= launchDistance)
        {
            StartRotationPhase();
        }
    }

    private void StartRotationPhase()//旋转阶段的准备活动
    {
        rb.linearVelocity = Vector2.zero;
        currentState = CrystalState.Rotating;
        rotationTimer = rotationDuration;

        // 随机选择旋转中心
        if (rotationCenters != null && rotationCenters.Length > 0)
        {
            int randomIndex = Random.Range(0, rotationCenters.Length);
            currentRotationCenter = rotationCenters[randomIndex].position;
        }
        else
        {
            currentRotationCenter = transform.position;
        }

        currentRotationAngle = 0f;//改动：随机中心


    }
    #endregion

    #region Rotation Phase
    /*
    private void UpdateRotationPhase()
    {
        Debug.Log("旋转");
        // 旋转动画
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        // 计时器
        rotationTimer -= Time.deltaTime;
        if (rotationTimer <= 0)
        {
            StartHomingPhase();
        }
    }
    */
    /*
private void UpdateRotationPhase()
{
    Debug.Log("旋转");

    // 每帧增加的角度
    float angleIncrement = rotationSpeed * Time.deltaTime;

    // 更新当前角度
    rotationAngle += angleIncrement;

    // 计算圆周运动的半径（可以根据需要调整）
    float radius = 1.0f; // 圆周运动的半径

    // 计算圆周运动的中心点（可以根据需要调整）
    Vector2 centerPoint = PlayerManager.instance.player.transform.position;

    // 计算当前点在圆周上的位置
    float x = centerPoint.x + radius * Mathf.Cos(rotationAngle * Mathf.Deg2Rad);
    float y = centerPoint.y + radius * Mathf.Sin(rotationAngle * Mathf.Deg2Rad);

    // 将物体移动到计算出的位置
    transform.position = new Vector2(x, y);

    // 计时器
    rotationTimer -= Time.deltaTime;
    if (rotationTimer <= 0)
    {
        StartHomingPhase();
    }
}
    */

    
    private void UpdateRotationPhase()//旋转过程，旋转目前的实现方法不太好，暂时弃用了
    {
        // 更新旋转角度
        currentRotationAngle += rotationSpeed * Time.deltaTime;

        // 计算圆周位置
        float radian = currentRotationAngle * Mathf.Deg2Rad;
        Vector2 offset = new Vector2(
            Mathf.Cos(radian) * rotationRadius,
            Mathf.Sin(radian) * rotationRadius
        );

        // 保持圆周运动
        transform.position = currentRotationCenter + offset;

        // 计时器
        rotationTimer -= Time.deltaTime;
        if (rotationTimer <= 0 || currentRotationAngle >= 360f)
        {
            StartHomingPhase();
        }

    }



    private void StartHomingPhase()//追踪准备，A*寻路，没找到目标会自爆
    {
        if (closestTarget != null)
        {
            seeker.StartPath(transform.position, closestTarget.position, OnPathComplete);
            currentState = CrystalState.Homing;
        }
        else
        {
            FinishCrystal();
        }
    }
    #endregion

    #region Homing Phase (A* Pathfinding)
    private void OnPathComplete(Path p)//Seeker开始寻路的参数
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void UpdateHomingPhase()//寻路过程
    {
        if (path == null) return;

        // 到达路径终点
        if (currentWaypoint >= path.vectorPath.Count)
        {
            FinishCrystal();
            return;
        }

        // 计算避开障碍物的方向
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 avoidDirection = GetObstacleAvoidance();

        // 混合路径方向和避障方向
        Vector2 finalDirection = (direction + avoidDirection * 0.5f).normalized;

        rb.linearVelocity = finalDirection * homingSpeed;

        // 检查是否到达当前路径点
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    private Vector2 GetObstacleAvoidance()//避障
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            rb.linearVelocity.normalized,
            2f,
            obstacleMask);

        if (hit.collider != null)
        {
            // 计算避开障碍物的方向
            Vector2 avoidDirection = Vector2.Perpendicular(hit.normal).normalized;
            return avoidDirection * (1 - hit.distance / 2f);
        }
        return Vector2.zero;
    }
    #endregion





    public void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                PlayerManager.instance.player.stats.DoDamage(hit.GetComponent<CharacterStats>(), multiplier);
                //hit.GetComponent<Enemy>().DamageEffect();//改动说明：使用带数值计算的伤害函数替换攻击效果
            }
            }
    }



    public void FinishCrystal()
    {
        rb.linearVelocity = new Vector2(0, 0);
        if (canExplode)
        {
            anim.SetTrigger("Explode");
            canGrow = true;
        }
        else
        {
            SelfDestory();
        }
    }

    public void SelfDestory() => Destroy(gameObject);

}
