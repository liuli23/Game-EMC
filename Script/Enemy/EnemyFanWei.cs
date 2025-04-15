using UnityEngine;

public class EnemyFanWei : MonoBehaviour
{
    [SerializeField] Enemy Enemy;
    [SerializeField] private bool flag = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("碰撞");
        // 检查碰撞对象是否是敌人
        //Debug.Log("碰撞对象名称: " + collision.gameObject.name);
        //Debug.Log("碰撞对象是否包含 Enemy: " + (collision.GetComponent<Enemy>() != null));
        //Debug.Log("碰撞对象是否包含 PlayerState: " + (collision.GetComponent<PlayerStats>() != null));
        if (collision.GetComponent<Player>() != null)
        {
            // 获取敌人的状态组件
            PlayerStats _target = collision.GetComponent<PlayerStats>();

            if (_target != null)
            {
                //Debug.Log("下劈");
                // 对敌人造成伤害
                Enemy.stats.DoDamage(_target,1.5f);

                // 触发屏幕震动效果
                if (flag)
                {
                    PlayerManager.instance.player.fx.ScreenShake(
                        0.2f,
                        Random.Range(0.6f, 1.2f),
                        Random.Range(0.6f, 1.2f),
                        0
                    );
                    //PlayerManager.instance.player.SetVelocity(PlayerManager.instance.player.rb.linearVelocityX, PlayerManager.instance.player.xiaPiSpeed);

                    TimeScaleManager.instance.BulletTime();
                }
                flag = false;//防止反复

                // 触发武器的特殊效果（假设 Effect 方法已定义）
                //Inventory.instance.GetEquipment(EquipmentType.Weapon)?.Effect(_target.transform);
            }
        }

        flag = true;
    }
}
