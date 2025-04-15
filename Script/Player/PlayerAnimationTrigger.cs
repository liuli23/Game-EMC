using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    
    private void AnimationTrigger()
    {
        player.AnimationTrigger();

        //player.GetComponentInChildren<>


    }

    /*
    private void NormalAttacking()
    {
        player.NormalAttacking();
    }
    */

    private void AttackFlip()
    {
        player.canFlip=!player.canFlip;
        //player.canDash=!player.canDash;
    }

    private void AttackTrigger()
    {
        //AudioManager.instance.PlaySFX(12);//攻击音效
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        bool karouFlag = false;

        foreach(var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();

                player.stats.DoDamage(_target);

                player.fx.ScreenShake(.2f, Random.Range(0.6f,1.2f), Random.Range(0.6f, 1.2f), 0);

                Inventory.instance.GetEquipment(EquipmentType.Weapon)?.Effect(_target.transform);

                karouFlag=true;

                //hit.GetComponent<Enemy>().DamageEffect();//负责视觉效果，闪白
                //hit.GetComponent<CharacterStats>().MakeDamage(player.stats.damage.GetValue());
            }
        }

        if (karouFlag)
        {
            TimeScaleManager.instance.BulletTime();
            karouFlag = false ;
        }

}

    private void FireBullet()
    {
        SkillManager.instance.fire.UseSkill();//改动说明：统一技能使用格式，冷却逻辑在状态切换进行
    }

    private void BlackholeFire()
    {
        SkillManager.instance.blackhole.UseSkill();
    }


    private void XiaPiTrigger()
    {
        player.xiaPiState.XiaPiFanWei();
    }

}
