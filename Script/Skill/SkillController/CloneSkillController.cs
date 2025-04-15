using UnityEngine;
using UnityEngine.Pool;


public class CloneSkillController : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorloosingSpeed;

    //[SerializeField] private float cloneDuration;//�Ķ�ת�Ƶ�Clone���ܽű���
    private float cloneTimer;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .99f;
    [SerializeField] private float attackMultiplier;
    private Transform closetEnemy;

    // ��ӳ�����
    private ObjectPool<GameObject> clonePool;
    private bool isReleased; // ��ֹ�ظ��ͷ�

    public void SetPool(ObjectPool<GameObject> pool) => clonePool = pool;

    public void ResetState()
    {
        // ��������״̬
        sr.color = new Color(1, 1, 1, 1);
        cloneTimer = 0;
        isReleased = false;
        anim.Rebind();
        anim.Update(0f);
    }

    private void ReleaseClone()
    {
        if (clonePool != null && !isReleased)
        {
            isReleased = true;
            clonePool.Release(gameObject);
        }
    }



    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isReleased) return;

        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorloosingSpeed));

            if (sr.color.a <= 0 && !isReleased)
            {
                ReleaseClone();
            }
        }
    }


    public void SetupClone(Transform _newTransform,float _cloneDuration,bool _canAttack,Transform _closestEnemy,float _Mutiplier)
    {
        if (_canAttack)
        {
            anim.SetInteger("AttackNumber", Random.Range(1, 3));
            AudioManager.instance.PlaySFX(12, null, true, 1.8f, 2.4f);

        }
        transform.rotation = _newTransform.rotation;
        transform.position = _newTransform.position;
        cloneTimer = _cloneDuration;
        closetEnemy = _closestEnemy;
        attackMultiplier = _Mutiplier;

        FaceClosestTarget();
    }


    // �޸Ķ������������߼�
    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
        ReleaseClone(); // �����ͷ�
    }

    /*
    private void NormalAttacking()
    {
        player.NormalAttacking();
    }
    */

    private void AttackFlip()
    {
        //player.canFlip = !player.canFlip;
        //player.canDash=!player.canDash;
    }



    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                PlayerManager.instance.player.stats.DoDamage(hit.GetComponent<CharacterStats>(), attackMultiplier);
                //hit.GetComponent<Enemy>().DamageEffect();//�Ķ�˵����ʹ�ô���ֵ������˺������滻����Ч��

                if(PlayerManager.instance.player.skill.clone.canCloneUseEffect)
                {
                    ItemDataEquipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);
                    if (weaponData != null)
                        weaponData.Effect(hit.transform);
                }

            }
        }
    }


    private void FaceClosestTarget()
    {
        /*
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5);

        float closetDistance = Mathf.Infinity;

        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if(distanceToEnemy < closetDistance)
                {
                    closetDistance = distanceToEnemy;
                    closetEnemy = hit.transform;
                }
            }
        }
        */
        //�Ķ�˵�����ƶ���skill��
        if(closetEnemy != null)
        {
            //Debug.Log("closet");
            if (transform.position.x > closetEnemy.position.x && PlayerManager.instance.player.facingDirection==1)
            {
                transform.Rotate(0, 180, 0);
                //Debug.Log("��ת");
            }
            else if(transform.position.x < closetEnemy.position.x && PlayerManager.instance.player.facingDirection == -1)
            {
                transform.Rotate(0, 180, 0);
            }
        }


    }


}
