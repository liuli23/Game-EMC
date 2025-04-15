using UnityEngine;

public class Skill : MonoBehaviour
{
    public float cooldown;
    [SerializeField]protected float cooldownTimer;

    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
        CheckUnlock();
    }

    protected virtual void Update()
    {
        cooldownTimer  -= Time.deltaTime;
    }

    protected virtual void CheckUnlock()
    {

    }


    public virtual bool CanUseSkill()
    {
        if(cooldownTimer < 0 )
        {
            //UseSkill();
            cooldownTimer = cooldown;
            return true;
        }

        player.fx.CreateText("��ȴ");
        //Debug.Log("��ȴ");
        return false;
    }
    public virtual void UseSkill()
    {
        //�ż��ܣ�
    }

    protected virtual Transform FindClosestEnemy(Transform _checkTransform,float _checkDistance)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, _checkDistance);
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;


        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }
        return closestEnemy;
    }

}
