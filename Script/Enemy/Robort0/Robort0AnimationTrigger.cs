using UnityEngine;

public class Robort0AnimationTrigger : MonoBehaviour
{
    private EnemyRobort0 enemyRobort0 =>GetComponentInParent<EnemyRobort0>();

    private void AnimationTrigger()
    {
        //Debug.Log("´¥·¢¶¯»­´¥·¢Æ÷");
        enemyRobort0.AnimationTrigger();
        
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemyRobort0.attackCheck.position, enemyRobort0.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats _target = hit.GetComponent<PlayerStats>();
                enemyRobort0.stats.DoDamage(_target);
                enemyRobort0.fx.ScreenShake();
                AudioManager.instance.PlaySFX(17,enemyRobort0.transform);
                //hit.GetComponent<Player>().DamageEffect();
            }
        }
    }

    private void OpenCounterWindow()
    {
        enemyRobort0.OpenCounterAttackWindow();
        //enemyRobort0.CounterAttackImage.SetActive(true);
    }
    private void CloseCounterWindow()
    {
        enemyRobort0.CloseCounterAttackWindow();
        //enemyRobort0.CounterAttackImage.SetActive(false);
    }
}
