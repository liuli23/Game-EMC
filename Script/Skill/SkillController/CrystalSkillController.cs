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

    //�½�
    private Seeker seeker;
    private Path path;
    private int currentWaypoint;
    // ״̬����
    private CrystalState currentState = CrystalState.Launching;
    // �˶�����
    [Header("Launch Phase")]
    [SerializeField] private float launchSpeed = 10f;
    [SerializeField] private float launchDistance = 3f;
    private Vector2 launchDirection;

    [Header("Rotation Phase")]
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float rotationDuration = 1f;
    [SerializeField]private float rotationAngle;


    [SerializeField] private Transform[] rotationCenters; // ͨ��Inspector���õ�Բ��λ��
    [SerializeField] private float rotationRadius = 1f;  // Բ���˶��뾶

    private Vector2 currentRotationCenter; // ��ǰ���ѡ���Բ��
    private float currentRotationAngle;    // ��ǰ��ת�Ƕ�


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
        // ���ҳ��������д���ǩ������
        GameObject[] centers = GameObject.FindGameObjectsWithTag("RotationCenter");
        rotationCenters = new Transform[centers.Length];

        for (int i = 0; i < centers.Length; i++)
        {
            rotationCenters[i] = centers[i].transform;
        }

        // ��֤����������Բ��
        if (rotationCenters.Length < 3)
            Debug.LogError("��������Ҫ���������� RotationCenter ��ǩ������");
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
        // ��ʼ������׶�
        InitLaunchPhase();


    }
    
    private void InitLaunchPhase()//��ʼ������״̬�Լ�����Ƕ�
    {
        /*
        // ����45��б�󷽷��䷽�򣨼�������泯�Ҳࣩ
        launchDirection = (new Vector2(-PlayerManager.instance.player.facingDirection,0) + Vector2.up).normalized;
        rb.linearVelocity = launchDirection * launchSpeed;
        currentState = CrystalState.Launching;
        */


        // ������ɷ���Ƕȣ���ɫ�󷽵��Ϸ�90�ȷ�Χ��
        float minAngle = 135f; // ���󷽣�180�ȣ������Ϸ���90�ȣ�֮��ķ�Χ
        float maxAngle = 90f;
        float randomAngle = Random.Range(minAngle, maxAngle);

        // ������ҳ�������Ƕȷ���
        if (PlayerManager.instance.player.facingDirection < 0)
            randomAngle = 180f - randomAngle;

        launchDirection = Quaternion.Euler(0, 0, randomAngle) * Vector2.right;
        rb.linearVelocity = launchDirection * launchSpeed;
        currentState = CrystalState.Launching;
        //�Ķ�˵������Χ��ĳ������Ƕ�

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

    private void StartRotationPhase()//��ת�׶ε�׼���
    {
        rb.linearVelocity = Vector2.zero;
        currentState = CrystalState.Rotating;
        rotationTimer = rotationDuration;

        // ���ѡ����ת����
        if (rotationCenters != null && rotationCenters.Length > 0)
        {
            int randomIndex = Random.Range(0, rotationCenters.Length);
            currentRotationCenter = rotationCenters[randomIndex].position;
        }
        else
        {
            currentRotationCenter = transform.position;
        }

        currentRotationAngle = 0f;//�Ķ����������


    }
    #endregion

    #region Rotation Phase
    /*
    private void UpdateRotationPhase()
    {
        Debug.Log("��ת");
        // ��ת����
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        // ��ʱ��
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
    Debug.Log("��ת");

    // ÿ֡���ӵĽǶ�
    float angleIncrement = rotationSpeed * Time.deltaTime;

    // ���µ�ǰ�Ƕ�
    rotationAngle += angleIncrement;

    // ����Բ���˶��İ뾶�����Ը�����Ҫ������
    float radius = 1.0f; // Բ���˶��İ뾶

    // ����Բ���˶������ĵ㣨���Ը�����Ҫ������
    Vector2 centerPoint = PlayerManager.instance.player.transform.position;

    // ���㵱ǰ����Բ���ϵ�λ��
    float x = centerPoint.x + radius * Mathf.Cos(rotationAngle * Mathf.Deg2Rad);
    float y = centerPoint.y + radius * Mathf.Sin(rotationAngle * Mathf.Deg2Rad);

    // �������ƶ����������λ��
    transform.position = new Vector2(x, y);

    // ��ʱ��
    rotationTimer -= Time.deltaTime;
    if (rotationTimer <= 0)
    {
        StartHomingPhase();
    }
}
    */

    
    private void UpdateRotationPhase()//��ת���̣���תĿǰ��ʵ�ַ�����̫�ã���ʱ������
    {
        // ������ת�Ƕ�
        currentRotationAngle += rotationSpeed * Time.deltaTime;

        // ����Բ��λ��
        float radian = currentRotationAngle * Mathf.Deg2Rad;
        Vector2 offset = new Vector2(
            Mathf.Cos(radian) * rotationRadius,
            Mathf.Sin(radian) * rotationRadius
        );

        // ����Բ���˶�
        transform.position = currentRotationCenter + offset;

        // ��ʱ��
        rotationTimer -= Time.deltaTime;
        if (rotationTimer <= 0 || currentRotationAngle >= 360f)
        {
            StartHomingPhase();
        }

    }



    private void StartHomingPhase()//׷��׼����A*Ѱ·��û�ҵ�Ŀ����Ա�
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
    private void OnPathComplete(Path p)//Seeker��ʼѰ·�Ĳ���
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void UpdateHomingPhase()//Ѱ·����
    {
        if (path == null) return;

        // ����·���յ�
        if (currentWaypoint >= path.vectorPath.Count)
        {
            FinishCrystal();
            return;
        }

        // ����ܿ��ϰ���ķ���
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 avoidDirection = GetObstacleAvoidance();

        // ���·������ͱ��Ϸ���
        Vector2 finalDirection = (direction + avoidDirection * 0.5f).normalized;

        rb.linearVelocity = finalDirection * homingSpeed;

        // ����Ƿ񵽴ﵱǰ·����
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    private Vector2 GetObstacleAvoidance()//����
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            rb.linearVelocity.normalized,
            2f,
            obstacleMask);

        if (hit.collider != null)
        {
            // ����ܿ��ϰ���ķ���
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
                //hit.GetComponent<Enemy>().DamageEffect();//�Ķ�˵����ʹ�ô���ֵ������˺������滻����Ч��
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
